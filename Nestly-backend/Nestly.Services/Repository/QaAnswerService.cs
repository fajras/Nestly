using Microsoft.EntityFrameworkCore;
using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;
using Nestly.Model.SearchObjects;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class QaAnswerService : IQaAnswerService
    {
        private readonly NestlyDbContext _db;
        public QaAnswerService(NestlyDbContext db) => _db = db;

        public List<QaAnswer> Get(QaAnswerSearchObject? search)
        {
            IQueryable<QaAnswer> q = _db.QaAnswers
                                        .Include(a => a.Question)
                                        .Include(a => a.AnsweredBy)
                                        .AsQueryable();

            if (search?.QuestionId is not null)
            {
                q = q.Where(a => a.QuestionId == search.QuestionId);
            }

            if (search?.AnsweredByUserId is not null)
            {
                q = q.Where(a => a.AnsweredByUserId == search.AnsweredByUserId);
            }

            if (search?.From is not null)
            {
                q = q.Where(a => a.CreatedAt >= search.From.Value);
            }

            if (search?.To is not null)
            {
                q = q.Where(a => a.CreatedAt <= search.To.Value);
            }

            return q.OrderBy(a => a.CreatedAt).ToList();
        }

        public QaAnswer? GetById(long id)
        {
            return _db.QaAnswers
                      .Include(a => a.Question)
                      .Include(a => a.AnsweredBy)
                      .FirstOrDefault(a => a.Id == id);
        }

        public QaAnswer Create(QaAnswer entity)
        {
            if (string.IsNullOrWhiteSpace(entity.AnswerText))
            {
                throw new ArgumentException("AnswerText is required.");
            }

            if (!_db.QaQuestions.Any(q => q.Id == entity.QuestionId))
            {
                throw new ArgumentException("Question does not exist.");
            }

            if (entity.AnsweredByUserId is not null &&
                !_db.AppUsers.Any(u => u.Id == entity.AnsweredByUserId.Value))
            {
                throw new ArgumentException("AnsweredBy user does not exist.");
            }

            if (entity.CreatedAt == default)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }

            _db.QaAnswers.Add(entity);
            _db.SaveChanges();
            return entity;
        }

        public QaAnswer? Patch(long id, QaAnswerPatchDto patch)
        {
            var a = _db.QaAnswers.FirstOrDefault(x => x.Id == id);
            if (a is null)
            {
                return null;
            }

            if (patch.AnswerText is not null)
            {
                a.AnswerText = patch.AnswerText;
            }

            if (patch.AnsweredByUserId is not null && patch.AnsweredByUserId != a.AnsweredByUserId)
            {
                if (!_db.AppUsers.Any(u => u.Id == patch.AnsweredByUserId.Value))
                {
                    throw new ArgumentException("AnsweredBy user does not exist.");
                }

                a.AnsweredByUserId = patch.AnsweredByUserId.Value;
            }

            _db.SaveChanges();
            return a;
        }

        public bool Delete(long id)
        {
            var a = _db.QaAnswers.FirstOrDefault(x => x.Id == id);
            if (a is null)
            {
                return false;
            }

            _db.QaAnswers.Remove(a);
            _db.SaveChanges();
            return true;
        }
    }
}
