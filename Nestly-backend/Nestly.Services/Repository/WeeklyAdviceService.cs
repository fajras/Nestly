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

        public PagedResult<WeeklyAdviceResponseDto> Get(WeeklyAdviceSearchObject search)
        {
            var query = _db.WeeklyAdvices
                .AsNoTracking()
                .AsQueryable();

            if (search.WeekNumber.HasValue)
            {
                query = query.Where(w => w.WeekNumber == search.WeekNumber);
            }

            var totalCount = query.Count();

            var items = query
                .OrderBy(w => w.WeekNumber)
                .Skip((search.Page - 1) * search.PageSize)
                .Take(search.PageSize)
                .Select(ToDto)
                .ToList();

            return new PagedResult<WeeklyAdviceResponseDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public WeeklyAdviceResponseDto GetById(int id)
        {
            var entity = _db.WeeklyAdvices
                .AsNoTracking()
                .FirstOrDefault(w => w.Id == id);

            if (entity == null)
            {
                throw new NotFoundException("Weekly advice not found.");
            }

            return ToDto(entity);
        }

        public WeeklyAdviceResponseDto GetByWeek(short weekNumber)
        {
            var entity = _db.WeeklyAdvices
                .AsNoTracking()
                .FirstOrDefault(w => w.WeekNumber == weekNumber);

            if (entity == null)
            {
                throw new NotFoundException($"Advice for week {weekNumber} not found.");
            }

            return ToDto(entity);
        }

        public WeeklyAdviceResponseDto Create(CreateWeeklyAdviceDto dto)
        {
            if (dto.WeekNumber <= 0)
            {
                throw new BusinessException("WeekNumber must be greater than 0.");
            }

            if (string.IsNullOrWhiteSpace(dto.AdviceText))
            {
                throw new BusinessException("AdviceText is required.");
            }

            if (_db.WeeklyAdvices.Any(w => w.WeekNumber == dto.WeekNumber))
            {
                throw new BusinessException($"Advice for week {dto.WeekNumber} already exists.");
            }

            var entity = new WeeklyAdvice
            {
                WeekNumber = dto.WeekNumber,
                AdviceText = dto.AdviceText.Trim()
            };

            _db.WeeklyAdvices.Add(entity);
            _db.SaveChanges();

            return ToDto(entity);
        }

        public WeeklyAdviceResponseDto Patch(int id, WeeklyAdvicePatchDto patch)
        {
            var entity = _db.WeeklyAdvices.FirstOrDefault(w => w.Id == id);

            if (entity == null)
            {
                throw new NotFoundException("Weekly advice not found.");
            }

            if (patch.WeekNumber.HasValue)
            {
                if (patch.WeekNumber <= 0)
                {
                    throw new BusinessException("WeekNumber must be greater than 0.");
                }

                bool exists = _db.WeeklyAdvices.Any(w =>
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

            _db.SaveChanges();
            return ToDto(entity);
        }

        public void Delete(int id)
        {
            var entity = _db.WeeklyAdvices.FirstOrDefault(w => w.Id == id);

            if (entity == null)
            {
                throw new NotFoundException("Weekly advice not found.");
            }

            _db.WeeklyAdvices.Remove(entity);
            _db.SaveChanges();
        }
    }
}
