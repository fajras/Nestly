using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class CalendarEventService : ICalendarEventService
    {
        private readonly NestlyDbContext _db;
        public CalendarEventService(NestlyDbContext db) => _db = db;

        public List<CalendarEvent> Get(CalendarEventSearchObject? search)
        {
            IQueryable<CalendarEvent> q = _db.CalendarEvents
                .Include(e => e.Baby)
                .Include(e => e.User)
                .AsQueryable();

            if (search?.BabyId is not null)
            {
                q = q.Where(e => e.BabyId == search.BabyId);
            }

            if (search?.UserId is not null)
            {
                q = q.Where(e => e.UserId == search.UserId);
            }

            if (search?.From is not null)
            {
                q = q.Where(e => e.StartAt >= search.From.Value);
            }

            if (search?.To is not null)
            {
                q = q.Where(e => e.StartAt <= search.To.Value);
            }

            if (!string.IsNullOrWhiteSpace(search?.Title))
            {
                q = q.Where(e => e.Title.Contains(search.Title));
            }

            return q.OrderBy(e => e.StartAt).ToList();
        }

        public CalendarEvent? GetById(long id)
        {
            return _db.CalendarEvents
                     .Include(e => e.Baby)
                     .Include(e => e.User)
                     .FirstOrDefault(e => e.Id == id);
        }

        public CalendarEvent Create(CreateCalendarEventDto dto)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (dto.BabyId <= 0)
            {
                throw new ArgumentException("BabyId is required.", nameof(dto.BabyId));
            }

            var babyParentId = _db.BabyProfiles
                .Where(b => b.Id == dto.BabyId)
                .Select(b => (long?)b.ParentProfileId)
                .FirstOrDefault();

            if (babyParentId is null)
            {
                throw new ArgumentException("Baby does not exist.", nameof(dto.BabyId));
            }

            if (dto.UserId.HasValue)
            {
                var parentExists = _db.ParentProfiles.Any(p => p.Id == dto.UserId.Value);
                if (!parentExists)
                {
                    throw new ArgumentException("Parent profile (UserId) does not exist.", nameof(dto.UserId));
                }

                if (babyParentId.Value != dto.UserId.Value)
                {
                    throw new InvalidOperationException("The specified parent profile does not own this baby.");
                }
            }

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new ArgumentException("Title is required.", nameof(dto.Title));
            }

            if (dto.StartAt == default)
            {
                throw new ArgumentException("StartAt is required.", nameof(dto.StartAt));
            }

            if (dto.EndAt is not null && dto.EndAt < dto.StartAt)
            {
                throw new ArgumentException("EndAt must be >= StartAt.", nameof(dto.EndAt));
            }

            var entity = new CalendarEvent
            {
                BabyId = dto.BabyId,
                UserId = dto.UserId,
                Title = dto.Title.Trim(),
                Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(),
                StartAt = dto.StartAt,
                EndAt = dto.EndAt
            };

            _db.CalendarEvents.Add(entity);
            _db.SaveChanges();

            return entity;
        }

        public CalendarEvent? Patch(long id, CalendarEventPatchDto patch)
        {
            var ev = _db.CalendarEvents.FirstOrDefault(x => x.Id == id);
            if (ev is null)
            {
                return null;
            }


            if (patch.Title is not null)
            {
                ev.Title = patch.Title;
            }

            if (patch.Description is not null)
            {
                ev.Description = patch.Description;
            }

            if (patch.StartAt is not null)
            {
                ev.StartAt = patch.StartAt.Value;
            }

            if (patch.EndAt is not null)
            {
                ev.EndAt = patch.EndAt;
            }

            if (ev.EndAt is not null && ev.EndAt < ev.StartAt)
            {
                throw new ArgumentException("EndAt must be >= StartAt.");
            }

            _db.SaveChanges();
            return ev;
        }

        public bool Delete(long id)
        {
            var ev = _db.CalendarEvents.FirstOrDefault(x => x.Id == id);
            if (ev is null)
            {
                return false;
            }

            _db.CalendarEvents.Remove(ev);
            _db.SaveChanges();
            return true;
        }

    }
}
