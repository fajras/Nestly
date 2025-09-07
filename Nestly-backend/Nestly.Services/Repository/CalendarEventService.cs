using Microsoft.EntityFrameworkCore;
using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;
using Nestly.Model.SearchObjects;
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

        public CalendarEvent Create(CalendarEvent entity)
        {
            if (entity.BabyId <= 0)
            {
                throw new ArgumentException("BabyId is required.");
            }

            if (!_db.BabyProfiles.Any(b => b.Id == entity.BabyId))
            {
                throw new ArgumentException("Baby does not exist.");
            }

            if (entity.UserId is not null && !_db.AppUsers.Any(u => u.Id == entity.UserId.Value))
            {
                throw new ArgumentException("User does not exist.");
            }

            if (entity.EndAt is not null && entity.EndAt < entity.StartAt)
            {
                throw new ArgumentException("EndAt must be >= StartAt.");
            }

            if (entity.CreatedAt == default)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }

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
