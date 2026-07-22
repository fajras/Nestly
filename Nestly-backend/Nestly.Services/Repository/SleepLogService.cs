using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Exceptions;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class SleepLogService : ISleepLogService
    {
        private readonly NestlyDbContext _db;

        public SleepLogService(NestlyDbContext db)
        {
            _db = db;
        }

        private static SleepLogResponseDto MapToDto(SleepLog entity)
        {
            return new SleepLogResponseDto
            {
                Id = entity.Id,
                BabyId = entity.BabyId,
                SleepDate = entity.SleepDate,
                StartTime = entity.StartTime.ToString(@"hh\:mm"),
                EndTime = entity.EndTime.ToString(@"hh\:mm"),
                DurationMinutes = entity.DurationMinutes
            };
        }

        // Sleep can span midnight (EndTime < StartTime wraps to the next
        // day), so overlap detection needs the absolute start/end instant
        // rather than comparing raw TimeSpans.
        private static (DateTime Start, DateTime End) GetAbsoluteInterval(
            DateTime date, TimeSpan start, TimeSpan end)
        {
            var absStart = date.Date + start;
            var absEnd = end >= start
                ? date.Date + end
                : date.Date.AddDays(1) + end;

            return (absStart, absEnd);
        }

        public async Task<PagedResult<SleepLogResponseDto>> Get(SleepLogSearchObject search)
        {
            IQueryable<SleepLog> q = _db.SleepLogs.AsNoTracking();

            if (search.BabyId is not null)
            {
                q = q.Where(x => x.BabyId == search.BabyId);
            }

            if (search.DateFrom is not null)
            {
                q = q.Where(x => x.SleepDate >= search.DateFrom.Value.Date);
            }

            if (search.DateTo is not null)
            {
                q = q.Where(x => x.SleepDate <= search.DateTo.Value.Date);
            }

            var totalCount = await q.CountAsync();
            int page = search.Page < 1 ? 1 : search.Page;

            int pageSize = search.PageSize < 1
                ? 10
                : search.PageSize > 100
                    ? 100
                    : search.PageSize;
            var entities = await q
                .OrderByDescending(x => x.SleepDate)
                .ThenByDescending(x => x.StartTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = entities.Select(MapToDto).ToList();

            return new PagedResult<SleepLogResponseDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }
        public async Task<SleepLogResponseDto> GetById(long id)
        {
            var entity = await _db.SleepLogs
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                throw new NotFoundException("Sleep log not found.");
            }

            return MapToDto(entity);
        }

        public async Task<SleepLogResponseDto> Create(CreateSleepLogDto dto)
        {
            if (!await _db.BabyProfiles.AnyAsync(b => b.Id == dto.BabyId))
            {
                throw new NotFoundException("Baby profile not found.");
            }

            if (dto.SleepDate.Date > DateTime.UtcNow.Date)
            {
                throw new BusinessException("Sleep date cannot be in the future.");
            }

            if (!TimeSpan.TryParse(dto.StartTime, out var start))
            {
                throw new BusinessException("Invalid start time format.");
            }

            if (!TimeSpan.TryParse(dto.EndTime, out var end))
            {
                throw new BusinessException("Invalid end time format.");
            }

            if (start == end)
            {
                throw new BusinessException("Start time and end time cannot be the same.");
            }

            await EnsureNoOverlap(dto.BabyId, dto.SleepDate.Date, start, end, excludeId: null);

            var entity = new SleepLog
            {
                BabyId = dto.BabyId,
                SleepDate = dto.SleepDate.Date,
                StartTime = start,
                EndTime = end
            };

            _db.SleepLogs.Add(entity);
            await _db.SaveChangesAsync();

            return MapToDto(entity);
        }
        public async Task<SleepLogResponseDto> Patch(long id, SleepLogPatchDto patch)
        {
            var entity = await _db.SleepLogs.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                throw new NotFoundException("Sleep log not found.");
            }

            var newDate = entity.SleepDate;
            var newStart = entity.StartTime;
            var newEnd = entity.EndTime;

            if (patch.SleepDate is not null)
            {
                if (patch.SleepDate.Value.Date > DateTime.UtcNow.Date)
                {
                    throw new BusinessException("Sleep date cannot be in the future.");
                }

                newDate = patch.SleepDate.Value.Date;
            }

            if (patch.StartTime is not null)
            {
                if (!TimeSpan.TryParse(patch.StartTime, out newStart))
                {
                    throw new BusinessException("Invalid start time format.");
                }
            }

            if (patch.EndTime is not null)
            {
                if (!TimeSpan.TryParse(patch.EndTime, out newEnd))
                {
                    throw new BusinessException("Invalid end time format.");
                }
            }

            if (newStart == newEnd)
            {
                throw new BusinessException("Start time and end time cannot be the same.");
            }

            await EnsureNoOverlap(entity.BabyId, newDate, newStart, newEnd, excludeId: entity.Id);

            entity.SleepDate = newDate;
            entity.StartTime = newStart;
            entity.EndTime = newEnd;

            await _db.SaveChangesAsync();
            return MapToDto(entity);
        }

        private async Task EnsureNoOverlap(
            long babyId, DateTime date, TimeSpan start, TimeSpan end, long? excludeId)
        {
            var (newStart, newEnd) = GetAbsoluteInterval(date, start, end);

            // Only nearby days can possibly overlap a (potentially
            // overnight-spanning) new entry.
            var others = await _db.SleepLogs
                .Where(x => x.BabyId == babyId &&
                    x.SleepDate >= date.AddDays(-1) &&
                    x.SleepDate <= date.AddDays(1) &&
                    (excludeId == null || x.Id != excludeId.Value))
                .ToListAsync();

            foreach (var other in others)
            {
                var (otherStart, otherEnd) = GetAbsoluteInterval(
                    other.SleepDate, other.StartTime, other.EndTime);

                if (newStart < otherEnd && otherStart < newEnd)
                {
                    throw new BusinessException(
                        "This sleep entry overlaps with an existing entry for this baby.");
                }
            }
        }

        public async Task Delete(long id)
        {
            var entity = await _db.SleepLogs.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                throw new NotFoundException("Sleep log not found.");
            }

            _db.SleepLogs.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<PagedResult<SleepLogResponseDto>> GetByParent(
    long parentProfileId,
    SleepLogSearchObject search)
        {
            IQueryable<SleepLog> q = _db.SleepLogs
                .AsNoTracking()
                .Where(x =>
                    x.Baby.ParentProfileId ==
                    parentProfileId);

            if (search.BabyId is not null)
            {
                q = q.Where(x =>
                    x.BabyId == search.BabyId);
            }

            if (search.DateFrom is not null)
            {
                q = q.Where(x =>
                    x.SleepDate >=
                    search.DateFrom.Value.Date);
            }

            if (search.DateTo is not null)
            {
                q = q.Where(x =>
                    x.SleepDate <=
                    search.DateTo.Value.Date);
            }

            var totalCount = await q.CountAsync();

            int page = search.Page < 1
                ? 1
                : search.Page;

            int pageSize = search.PageSize < 1
                ? 10
                : search.PageSize > 100
                    ? 100
                    : search.PageSize;

            var entities = await q
                .OrderByDescending(x => x.SleepDate)
                .ThenByDescending(x => x.StartTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = entities.Select(MapToDto).ToList();

            return new PagedResult<SleepLogResponseDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }
    }
}
