using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Exceptions;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class HealthDeviationAlertService : IHealthDeviationAlertService
    {
        private readonly NestlyDbContext _db;

        public HealthDeviationAlertService(NestlyDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResult<HealthDeviationAlertResponseDto>> GetByParent(
            long parentProfileId, HealthDeviationAlertSearchObject search)
        {
            IQueryable<HealthDeviationAlert> q = _db.HealthDeviationAlerts
                .Include(a => a.Baby)
                .Where(a => a.Baby.ParentProfileId == parentProfileId);

            if (search.BabyId is not null)
            {
                q = q.Where(a => a.BabyId == search.BabyId);
            }

            if (search.IsResolved is not null)
            {
                q = q.Where(a => a.IsResolved == search.IsResolved);
            }

            var totalCount = await q.CountAsync();
            var page = search.Page < 1 ? 1 : search.Page;

            var pageSize = search.PageSize < 1
                ? 20
                : search.PageSize > 100
                    ? 100
                    : search.PageSize;

            var entities = await q
                .OrderByDescending(a => a.DetectedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = entities.Select(MapToDto).ToList();

            return new PagedResult<HealthDeviationAlertResponseDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public async Task<HealthDeviationAlertResponseDto> Resolve(long id)
        {
            var entity = await _db.HealthDeviationAlerts
                .Include(a => a.Baby)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (entity is null)
            {
                throw new NotFoundException("Health alert not found.");
            }

            entity.IsResolved = true;
            entity.ResolvedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return MapToDto(entity);
        }

        private static HealthDeviationAlertResponseDto MapToDto(HealthDeviationAlert entity)
        {
            return new HealthDeviationAlertResponseDto
            {
                Id = entity.Id,
                BabyId = entity.BabyId,
                BabyName = entity.Baby?.BabyName,
                ParameterType = entity.ParameterType,
                Severity = entity.Severity,
                Title = entity.Title,
                Message = entity.Message,
                Recommendation = entity.Recommendation,
                DetectedAt = entity.DetectedAt,
                PeriodFrom = entity.PeriodFrom,
                PeriodTo = entity.PeriodTo,
                IsResolved = entity.IsResolved,
                ResolvedAt = entity.ResolvedAt
            };
        }
    }
}
