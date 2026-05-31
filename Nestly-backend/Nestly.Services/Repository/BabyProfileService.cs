using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Exceptions;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class BabyProfileService : IBabyProfileService
    {
        private readonly NestlyDbContext _db;
        private readonly ICurrentUserService _currentUserService;

        public BabyProfileService(
            NestlyDbContext db,
            ICurrentUserService currentUserService)
        {
            _db = db;
            _currentUserService = currentUserService;
        }

        public async Task<PagedResult<BabyProfileSummaryDto>> Get(
            BabyProfileSearchObject search)
        {
            IQueryable<BabyProfile> q = _db.BabyProfiles.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search.BabyName))
            {
                q = q.Where(x =>
                    x.BabyName.Contains(search.BabyName.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(search.Gender))
            {
                q = q.Where(x =>
                    x.Gender == search.Gender.Trim());
            }

            if (search.BirthDateFrom is not null)
            {
                q = q.Where(x =>
                    x.BirthDate >= search.BirthDateFrom.Value.Date);
            }

            if (search.BirthDateTo is not null)
            {
                q = q.Where(x =>
                    x.BirthDate <= search.BirthDateTo.Value.Date);
            }

            var totalCount = await q.CountAsync();

            int page = search.Page < 1 ? 1 : search.Page;

            int pageSize = search.PageSize < 1
                ? 10
                : search.PageSize > 100
                    ? 100
                    : search.PageSize;

            var entities = await q
                .OrderByDescending(x => x.BirthDate)
                .ThenBy(x => x.BabyName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = entities
                .Select(MapToDto)
                .ToList();

            return new PagedResult<BabyProfileSummaryDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public async Task<BabyProfileSummaryDto> GetById(long id)
        {
            var entity = await _db.BabyProfiles
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                throw new NotFoundException(
                    "Baby profile not found.");
            }

            return MapToDto(entity);
        }

        public async Task<BabyProfileSummaryDto> Create(
            CreateBabyProfileDto dto)
        {
            if (dto is null)
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

            if (string.IsNullOrWhiteSpace(dto.BabyName))
            {
                throw new BusinessException(
                    "Baby name is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Gender))
            {
                throw new BusinessException(
                    "Gender is required.");
            }

            if (dto.BirthDate == default)
            {
                throw new BusinessException(
                    "Birth date is required.");
            }

            if (dto.PregnancyId.HasValue)
            {
                await _currentUserService
                    .EnsurePregnancyOwnershipAsync(
                        dto.PregnancyId.Value);
            }

            var entity = new BabyProfile
            {
                ParentProfileId = parent.Id,
                BabyName = dto.BabyName.Trim(),
                Gender = dto.Gender.Trim(),
                BirthDate = dto.BirthDate,
                PregnancyId = dto.PregnancyId
            };

            await _db.BabyProfiles.AddAsync(entity);

            await _db.SaveChangesAsync();

            return MapToDto(entity);
        }

        public async Task<BabyProfileSummaryDto> Patch(
            long id,
            BabyProfilePatchDto patch)
        {
            var dbEntity = await _db.BabyProfiles
                .FirstOrDefaultAsync(x => x.Id == id);

            if (dbEntity is null)
            {
                throw new NotFoundException(
                    "Baby profile not found.");
            }

            if (patch.BabyName is not null)
            {
                dbEntity.BabyName = patch.BabyName.Trim();
            }

            if (patch.Gender is not null)
            {
                dbEntity.Gender = patch.Gender.Trim();
            }

            if (patch.BirthDate is not null)
            {
                dbEntity.BirthDate = patch.BirthDate.Value;
            }

            await _db.SaveChangesAsync();

            return MapToDto(dbEntity);
        }

        public async Task Delete(long id)
        {
            var dbEntity = await _db.BabyProfiles
                .FirstOrDefaultAsync(x => x.Id == id);

            if (dbEntity is null)
            {
                throw new NotFoundException(
                    "Baby profile not found.");
            }

            _db.BabyProfiles.Remove(dbEntity);

            await _db.SaveChangesAsync();
        }

        public async Task<BabyProfileSummaryDto?> GetLatestByParent(
            long parentProfileId)
        {
            var entity = await _db.BabyProfiles
                .Where(x => x.ParentProfileId == parentProfileId)
                .OrderByDescending(x => x.BirthDate)
                .FirstOrDefaultAsync();

            return entity is null
                ? null
                : MapToDto(entity);
        }

        private static BabyProfileSummaryDto MapToDto(
            BabyProfile entity)
        {
            return new BabyProfileSummaryDto
            {
                Id = entity.Id,
                BabyName = entity.BabyName,
                Gender = entity.Gender,
                BirthDate = entity.BirthDate
            };
        }
    }
}