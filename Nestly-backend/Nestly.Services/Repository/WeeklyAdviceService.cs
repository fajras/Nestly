using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Exceptions;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class WeeklyAdviceService : IWeeklyAdviceService
    {
        private const short MaxWeekNumber = 42;

        private readonly NestlyDbContext _db;

        public WeeklyAdviceService(NestlyDbContext db)
        {
            _db = db;
        }

        private static WeeklyAdviceResponseDto ToDto(WeeklyAdvice w) => new()
        {
            Id = w.Id,
            WeekNumber = w.WeekNumber,
            AdviceText = w.AdviceText
        };

        public async Task<PagedResult<WeeklyAdviceResponseDto>> Get(WeeklyAdviceSearchObject search)
        {
            var query = _db.WeeklyAdvices
                .AsNoTracking()
                .AsQueryable();

            if (search.WeekNumber.HasValue)
            {
                query = query.Where(w => w.WeekNumber == search.WeekNumber);
            }

            int page = search.Page < 1 ? 1 : search.Page;

            int pageSize = search.PageSize < 1
                ? 10
                : search.PageSize > 100
                    ? 100
                    : search.PageSize;

            var totalCount = await query.CountAsync();

            var entities = await query
                .OrderBy(w => w.WeekNumber)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = entities.Select(ToDto).ToList();

            return new PagedResult<WeeklyAdviceResponseDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public async Task<WeeklyAdviceResponseDto> GetById(int id)
        {
            var entity = await _db.WeeklyAdvices
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == id);

            if (entity == null)
            {
                throw new NotFoundException("Weekly advice not found.");
            }

            return ToDto(entity);
        }

        public async Task<WeeklyAdviceResponseDto?> GetByWeek(short weekNumber)
        {
            var entity = await _db.WeeklyAdvices
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.WeekNumber == weekNumber);

            if (entity == null)
            {
                throw new NotFoundException($"Advice for week {weekNumber} not found.");
            }

            return ToDto(entity);
        }

        public async Task<WeeklyAdviceResponseDto> Create(CreateWeeklyAdviceDto dto)
        {
            if (dto.WeekNumber <= 0 || dto.WeekNumber > MaxWeekNumber)
            {
                throw new BusinessException($"WeekNumber must be between 1 and {MaxWeekNumber}.");
            }

            if (string.IsNullOrWhiteSpace(dto.AdviceText))
            {
                throw new BusinessException("AdviceText is required.");
            }

            if (await _db.WeeklyAdvices.AnyAsync(w => w.WeekNumber == dto.WeekNumber))
            {
                throw new BusinessException($"Advice for week {dto.WeekNumber} already exists.");
            }

            var entity = new WeeklyAdvice
            {
                WeekNumber = dto.WeekNumber,
                AdviceText = dto.AdviceText.Trim()
            };

            _db.WeeklyAdvices.Add(entity);
            await _db.SaveChangesAsync();

            return ToDto(entity);
        }

        public async Task<WeeklyAdviceResponseDto?> Patch(int id, WeeklyAdvicePatchDto patch)
        {
            var entity = await _db.WeeklyAdvices.FirstOrDefaultAsync(w => w.Id == id);

            if (entity == null)
            {
                throw new NotFoundException("Weekly advice not found.");
            }

            if (patch.WeekNumber.HasValue)
            {
                if (patch.WeekNumber <= 0 || patch.WeekNumber > MaxWeekNumber)
                {
                    throw new BusinessException($"WeekNumber must be between 1 and {MaxWeekNumber}.");
                }

                bool exists = await _db.WeeklyAdvices.AnyAsync(w =>
                    w.WeekNumber == patch.WeekNumber &&
                    w.Id != id);

                if (exists)
                {
                    throw new BusinessException("Another advice with this week already exists.");
                }

                entity.WeekNumber = patch.WeekNumber.Value;
            }

            if (patch.AdviceText is not null)
            {
                if (string.IsNullOrWhiteSpace(patch.AdviceText))
                {
                    throw new BusinessException("AdviceText cannot be empty.");
                }

                entity.AdviceText = patch.AdviceText.Trim();
            }

            await _db.SaveChangesAsync();
            return ToDto(entity);
        }

        public async Task Delete(int id)
        {
            var entity = await _db.WeeklyAdvices.FirstOrDefaultAsync(w => w.Id == id);

            if (entity == null)
            {
                throw new NotFoundException("Weekly advice not found.");
            }

            _db.WeeklyAdvices.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }
}
