using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Exceptions;

namespace Nestly.Services.Repository
{
    public class FeedingLogService : IFeedingLogService
    {
        private readonly NestlyDbContext _db;

        public FeedingLogService(NestlyDbContext db)
        {
            _db = db;
        }

        public PagedResult<FeedingLogResponseDto> Get(FeedingLogSearchObject search)
        {
            IQueryable<FeedingLog> q = _db.FeedingLogs
                .Include(f => f.FoodType)
                .AsQueryable();

            if (search.BabyId is not null)
            {
                q = q.Where(x => x.BabyId == search.BabyId);
            }

            if (search.DateFrom is not null)
            {
                q = q.Where(x => x.FeedDate >= search.DateFrom.Value);
            }

            if (search.DateTo is not null)
            {
                q = q.Where(x => x.FeedDate <= search.DateTo.Value);
            }

            var totalCount = q.Count();

            var items = q
                .OrderByDescending(x => x.FeedDate)
                .ThenByDescending(x => x.FeedTime)
                .Skip((search.Page - 1) * search.PageSize)
                .Take(search.PageSize)
                .Select(x => MapToDto(x))
                .ToList();

            return new PagedResult<FeedingLogResponseDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public FeedingLogResponseDto GetById(long id)
        {
            var entity = _db.FeedingLogs
                .Include(f => f.FoodType)
                .FirstOrDefault(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException("Feeding log not found.");
            }

            return MapToDto(entity);
        }

        public FeedingLogResponseDto Create(CreateFeedingLogDto dto)
        {
            if (dto is null)
            {
                throw new BusinessException("Request cannot be null.");
            }

            if (dto.BabyId <= 0)
            {
                throw new BusinessException("Baby is required.");
            }

            if (!_db.BabyProfiles.Any(b => b.Id == dto.BabyId))
            {
                throw new NotFoundException("Baby profile not found.");
            }

            if (dto.FeedDate == default)
            {
                throw new BusinessException("Feed date is required.");
            }

            if (dto.AmountMl is < 0)
            {
                throw new BusinessException("Amount cannot be negative.");
            }

            if (dto.FoodTypeId.HasValue &&
                !_db.FoodTypes.Any(f => f.Id == dto.FoodTypeId.Value))
            {
                throw new NotFoundException("Food type not found.");
            }

            var entity = new FeedingLog
            {
                BabyId = dto.BabyId,
                FeedDate = dto.FeedDate.Date,
                FeedTime = dto.FeedTime,
                AmountMl = dto.AmountMl,
                FoodTypeId = dto.FoodTypeId,
                Notes = string.IsNullOrWhiteSpace(dto.Notes)
                    ? null
                    : dto.Notes.Trim()
            };

            _db.FeedingLogs.Add(entity);
            _db.SaveChanges();

            return MapToDto(entity);
        }
        public FeedingLogResponseDto Patch(long id, FeedingLogPatchDto patch)
        {
            var dbEntity = _db.FeedingLogs
                .Include(f => f.FoodType)
                .FirstOrDefault(x => x.Id == id);

            if (dbEntity is null)
            {
                throw new NotFoundException("Feeding log not found.");
            }

            if (patch.FeedDate is not null)
            {
                dbEntity.FeedDate = patch.FeedDate.Value.Date;
            }

            if (patch.FeedTime is not null)
            {
                dbEntity.FeedTime = patch.FeedTime.Value;
            }

            if (patch.AmountMl is not null)
            {
                if (patch.AmountMl < 0)
                {
                    throw new BusinessException("Amount cannot be negative.");
                }

                dbEntity.AmountMl = patch.AmountMl.Value;
            }

            if (patch.FoodTypeId is not null)
            {
                if (!_db.FoodTypes.Any(f => f.Id == patch.FoodTypeId.Value))
                {
                    throw new NotFoundException("Food type not found.");
                }

                dbEntity.FoodTypeId = patch.FoodTypeId.Value;
            }

            if (patch.Notes is not null)
            {
                dbEntity.Notes = string.IsNullOrWhiteSpace(patch.Notes)
                    ? null
                    : patch.Notes.Trim();
            }

            _db.SaveChanges();

            return MapToDto(dbEntity);
        }
        public void Delete(long id)
        {
            var dbEntity = _db.FeedingLogs.FirstOrDefault(x => x.Id == id);

            if (dbEntity is null)
            {
                throw new NotFoundException("Feeding log not found.");
            }

            _db.FeedingLogs.Remove(dbEntity);
            _db.SaveChanges();
        }

        private static FeedingLogResponseDto MapToDto(FeedingLog x)
        {
            return new FeedingLogResponseDto
            {
                Id = x.Id,
                BabyId = x.BabyId,
                FeedDate = x.FeedDate,
                FeedTime = x.FeedTime,
                AmountMl = x.AmountMl,
                FoodTypeId = x.FoodTypeId,
                FoodTypeName = x.FoodType != null ? x.FoodType.Name : null,
                Notes = x.Notes
            };
        }
    }
}
