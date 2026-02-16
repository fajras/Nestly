using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;

namespace Nestly.Services.Repository
{
    public class MilestoneService : IMilestoneService
    {
        private readonly NestlyDbContext _db;
        public MilestoneService(NestlyDbContext db) => _db = db;

        public List<MilestoneResponseDto> Get(MilestoneSearchObject? search)
        {
            IQueryable<Milestone> q = _db.Milestones.AsNoTracking();

            if (search?.BabyId is not null)
            {
                q = q.Where(x => x.BabyId == search.BabyId);
            }

            if (search?.DateFrom is not null)
            {
                q = q.Where(x => x.AchievedDate >= search.DateFrom.Value);
            }

            if (search?.DateTo is not null)
            {
                q = q.Where(x => x.AchievedDate <= search.DateTo.Value);
            }

            if (!string.IsNullOrWhiteSpace(search?.Title))
            {
                q = q.Where(x => x.Title.Contains(search.Title));
            }

            return q
                .OrderBy(x => x.AchievedDate)
                .Select(ToDto)
                .ToList();
        }

        public MilestoneResponseDto? GetById(long id)
        {
            var entity = _db.Milestones
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id);

            return entity is null ? null : ToDto(entity);
        }


        public MilestoneResponseDto Create(CreateMilestoneDto dto)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (!_db.BabyProfiles.Any(b => b.Id == dto.BabyId))
            {
                throw new ArgumentException("Baby does not exist.", nameof(dto.BabyId));
            }

            var entity = new Milestone
            {
                BabyId = dto.BabyId,
                Title = dto.Title.Trim(),
                AchievedDate = dto.AchievedDate,
                Notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            _db.Milestones.Add(entity);
            _db.SaveChanges();

            return ToDto(entity);
        }

        public MilestoneResponseDto? Patch(long id, MilestonePatchDto patch)
        {
            var entity = _db.Milestones.FirstOrDefault(x => x.Id == id);
            if (entity is null)
            {
                return null;
            }

            if (patch.Title is not null)
            {
                entity.Title = patch.Title.Trim();
            }

            if (patch.AchievedDate is not null)
            {
                entity.AchievedDate = patch.AchievedDate.Value;
            }

            if (patch.Notes is not null)
            {
                entity.Notes = patch.Notes.Trim();
            }

            _db.SaveChanges();

            return ToDto(entity);
        }

        public bool Delete(long id)
        {
            var dbEntity = _db.Milestones.FirstOrDefault(x => x.Id == id);
            if (dbEntity is null)
            {
                return false;
            }

            _db.Milestones.Remove(dbEntity);
            _db.SaveChanges();
            return true;
        }

        private static MilestoneResponseDto ToDto(Milestone m) => new()
        {
            Id = m.Id,
            BabyId = m.BabyId,
            Title = m.Title,
            AchievedDate = m.AchievedDate,
            Notes = m.Notes,
            CreatedAt = m.CreatedAt
        };
    }
}

