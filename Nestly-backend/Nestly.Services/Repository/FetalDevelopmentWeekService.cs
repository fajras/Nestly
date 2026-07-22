using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Exceptions;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class FetalDevelopmentWeekService : IFetalDevelopmentWeekService
    {
        private const int MaxWeekNumber = 42;

        private readonly NestlyDbContext _db;

        public FetalDevelopmentWeekService(NestlyDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResult<FetalDevelopmentWeekResponseDto>> Get(FetalDevelopmentWeekSearchObject search)
        {
            IQueryable<FetalDevelopmentWeek> q = _db.FetalDevelopmentWeeks.AsQueryable();

            if (search.WeekNumber is not null)
            {
                q = q.Where(x => x.WeekNumber == search.WeekNumber.Value);
            }

            var totalCount = await q.CountAsync();
            int page = search.Page < 1 ? 1 : search.Page;

            int pageSize = search.PageSize < 1
                ? 10
                : search.PageSize > 100
                    ? 100
                    : search.PageSize;
            var entities = await q
                .OrderBy(x => x.WeekNumber)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = entities.Select(MapToDto).ToList();

            return new PagedResult<FetalDevelopmentWeekResponseDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public async Task<FetalDevelopmentWeekResponseDto> GetById(int id)
        {
            var entity = await _db.FetalDevelopmentWeeks
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException("Fetal development week not found.");
            }

            return MapToDto(entity);
        }

        public async Task<FetalDevelopmentWeekResponseDto?> GetByWeekNumber(int weekNumber)
        {
            var entity = await _db.FetalDevelopmentWeeks
                .FirstOrDefaultAsync(x => x.WeekNumber == weekNumber);

            return entity is null ? null : MapToDto(entity);
        }

        public async Task<FetalDevelopmentWeekResponseDto> Create(CreateFetalDevelopmentWeekDto dto)
        {
            if (dto is null)
            {
                throw new BusinessException("Request cannot be null.");
            }

            if (dto.WeekNumber <= 0 || dto.WeekNumber > MaxWeekNumber)
            {
                throw new BusinessException($"Week number must be between 1 and {MaxWeekNumber}.");
            }

            ValidateImageUrl(dto.ImageUrl);

            if (await _db.FetalDevelopmentWeeks.AnyAsync(x => x.WeekNumber == dto.WeekNumber))
            {
                throw new BusinessException($"Week {dto.WeekNumber} already exists.");
            }

            var entity = new FetalDevelopmentWeek
            {
                WeekNumber = dto.WeekNumber,
                ImageUrl = dto.ImageUrl?.Trim(),
                BabyDevelopment = dto.BabyDevelopment?.Trim(),
                MotherChanges = dto.MotherChanges?.Trim()
            };

            _db.FetalDevelopmentWeeks.Add(entity);
            await _db.SaveChangesAsync();

            return MapToDto(entity);
        }
        public async Task<FetalDevelopmentWeekResponseDto> Patch(int id, FetalDevelopmentWeekPatchDto patch)
        {
            var entity = await _db.FetalDevelopmentWeeks.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException("Fetal development week not found.");
            }

            if (patch.BabyDevelopment is not null)
            {
                entity.BabyDevelopment = patch.BabyDevelopment.Trim();
            }

            if (patch.MotherChanges is not null)
            {
                entity.MotherChanges = patch.MotherChanges.Trim();
            }

            if (patch.ImageUrl is not null)
            {
                ValidateImageUrl(patch.ImageUrl);
                entity.ImageUrl = patch.ImageUrl.Trim();
            }

            await _db.SaveChangesAsync();

            return MapToDto(entity);
        }

        public async Task Delete(int id)
        {
            var entity = await _db.FetalDevelopmentWeeks.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException("Fetal development week not found.");
            }

            _db.FetalDevelopmentWeeks.Remove(entity);
            await _db.SaveChangesAsync();
        }

        private static void ValidateImageUrl(string? imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return;
            }

            if (!Uri.TryCreate(imageUrl.Trim(), UriKind.Absolute, out var uri) ||
                (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                throw new BusinessException("Image URL must be a valid absolute http(s) URL.");
            }
        }

        private static FetalDevelopmentWeekResponseDto MapToDto(FetalDevelopmentWeek x)
        {
            return new FetalDevelopmentWeekResponseDto
            {
                Id = x.Id,
                WeekNumber = x.WeekNumber,
                ImageUrl = x.ImageUrl,
                BabyDevelopment = x.BabyDevelopment,
                MotherChanges = x.MotherChanges
            };
        }
    }
}
