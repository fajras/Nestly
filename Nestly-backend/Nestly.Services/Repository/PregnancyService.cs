using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Exceptions;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class PregnancyService : IPregnancyService
    {
        private readonly NestlyDbContext _db;
        private readonly ICurrentUserService _currentUserService;

        private const int GestationDays = 280;

        public PregnancyService(
            NestlyDbContext db,
            ICurrentUserService currentUserService)
        {
            _db = db;
            _currentUserService = currentUserService;
        }

        public async Task<PagedResult<PregnancyResponseDto>> Get(
            PregnancySearchObject search)
        {
            IQueryable<Pregnancy> q = _db.Pregnancies.AsNoTracking();

            if (search.LmpFrom is not null)
            {
                q = q.Where(p => p.LmpDate >= search.LmpFrom.Value);
            }

            if (search.LmpTo is not null)
            {
                q = q.Where(p => p.LmpDate <= search.LmpTo.Value);
            }

            if (search.DueFrom is not null)
            {
                q = q.Where(p => p.DueDate >= search.DueFrom.Value);
            }

            if (search.DueTo is not null)
            {
                q = q.Where(p => p.DueDate <= search.DueTo.Value);
            }

            var totalCount = await q.CountAsync();

            int page = search.Page < 1 ? 1 : search.Page;

            int pageSize = search.PageSize < 1
                ? 10
                : search.PageSize > 100
                    ? 100
                    : search.PageSize;

            var entities = await q
            .OrderByDescending(p => p.LmpDate ?? DateTime.MinValue)
            .ThenByDescending(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            var items = entities
                .Select(ToDto)
                .ToList();

            return new PagedResult<PregnancyResponseDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public async Task<PregnancyResponseDto> GetById(long id)
        {
            var entity = await _db.Pregnancies
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (entity == null)
            {
                throw new NotFoundException(
                    "Pregnancy not found.");
            }

            return ToDto(entity);
        }

        public async Task<PregnancyResponseDto?> GetByParentProfileId(
            long parentProfileId)
        {
            var entity = await _db.Pregnancies
                .AsNoTracking()
                .Where(p => p.ParentProfileId == parentProfileId)
                .OrderByDescending(p => p.LmpDate ?? p.DueDate)
                .FirstOrDefaultAsync();

            return entity is null
                ? null
                : ToDto(entity);
        }

        public async Task<PregnancyResponseDto> Create(
            CreatePregnancyDto dto)
        {
            if (dto == null)
            {
                throw new BusinessException(
                    "Request cannot be null.");
            }

            var parent = await _currentUserService
                .GetCurrentParentProfileAsync();

            if (parent == null)
            {
                throw new NotFoundException(
                    "Parent profile not found.");
            }

            var (lmp, due) = NormalizeDates(
                dto.LmpDate,
                dto.DueDate);

            if (lmp.HasValue &&
                due.HasValue &&
                due < lmp)
            {
                throw new BusinessException(
                    "Due date cannot be before LMP.");
            }

            if (dto.CycleLengthDays.HasValue &&
                (dto.CycleLengthDays < 20 ||
                 dto.CycleLengthDays > 40))
            {
                throw new BusinessException(
                    "Cycle length must be between 20 and 40.");
            }

            var entity = new Pregnancy
            {
                ParentProfileId = parent.Id,
                LmpDate = lmp,
                DueDate = due,
                CycleLengthDays = dto.CycleLengthDays
            };

            await _db.Pregnancies.AddAsync(entity);

            await _db.SaveChangesAsync();

            return ToDto(entity);
        }

        public async Task<PregnancyResponseDto> Patch(
            long id,
            PregnancyPatchDto patch)
        {
            var entity = await _db.Pregnancies
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                throw new NotFoundException(
                    "Pregnancy not found.");
            }

            var newLmp = patch.LmpDate ?? entity.LmpDate;

            var newDue = patch.DueDate ?? entity.DueDate;

            (newLmp, newDue) = NormalizeDates(
                newLmp,
                newDue);

            if (newLmp.HasValue &&
                newDue.HasValue &&
                newDue < newLmp)
            {
                throw new BusinessException(
                    "Due date cannot be before LMP.");
            }

            entity.LmpDate = newLmp;

            entity.DueDate = newDue;

            if (patch.CycleLengthDays is not null)
            {
                if (!newLmp.HasValue)
                {
                    throw new BusinessException(
                        "Cycle length requires LMP.");
                }

                if (patch.CycleLengthDays < 20 ||
                    patch.CycleLengthDays > 40)
                {
                    throw new BusinessException(
                        "Cycle length must be between 20 and 40.");
                }

                entity.CycleLengthDays =
                    patch.CycleLengthDays;
            }

            await _db.SaveChangesAsync();

            return ToDto(entity);
        }

        public async Task Delete(long id)
        {
            var entity = await _db.Pregnancies
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                throw new NotFoundException(
                    "Pregnancy not found.");
            }

            _db.Pregnancies.Remove(entity);

            await _db.SaveChangesAsync();
        }

        public async Task<PregnancyStatusDto?> GetStatus(
            long parentProfileId)
        {
            var pregnancy = await _db.Pregnancies
                .Where(p => p.ParentProfileId == parentProfileId)
                .OrderByDescending(p => p.LmpDate ?? p.DueDate)
                .FirstOrDefaultAsync();

            if (pregnancy == null)
            {
                return null;
            }

            var (lmp, due) = NormalizeDates(
                pregnancy.LmpDate,
                pregnancy.DueDate);

            if (!lmp.HasValue || !due.HasValue)
            {
                return null;
            }

            var today = DateTime.UtcNow.Date;

            var ageDays = Math.Max(
                0,
                (today - lmp.Value).Days);

            var week = Math.Max(
                1,
                (ageDays / 7) + 1);

            var remaining = Math.Max(
                0,
                (due.Value - today).Days);

            return new PregnancyStatusDto
            {
                ParentProfileId = parentProfileId,
                LmpDate = lmp,
                DueDate = due,
                GestationalWeek = week,
                DaysRemaining = remaining
            };
        }

        private static (DateTime? lmp, DateTime? due)
            NormalizeDates(DateTime? lmp, DateTime? due)
        {
            if (lmp.HasValue && !due.HasValue)
            {
                return (
                    lmp,
                    lmp.Value.AddDays(GestationDays));
            }

            if (!lmp.HasValue && due.HasValue)
            {
                return (
                    due.Value.AddDays(-GestationDays),
                    due);
            }

            return (lmp, due);
        }

        private static PregnancyResponseDto ToDto(Pregnancy p)
            => new()
            {
                Id = p.Id,
                ParentProfileId = p.ParentProfileId,
                LmpDate = p.LmpDate,
                DueDate = p.DueDate,
                CycleLengthDays = p.CycleLengthDays
            };
    }
}