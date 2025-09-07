using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
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
                q = q.Where(a => a.AnsweredById == search.AnsweredByUserId);
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

        public QaAnswer Create(CreateQaAnswerDto dto)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (string.IsNullOrWhiteSpace(dto.AnswerText))
            {
                throw new ArgumentException("AnswerText is required.", nameof(dto.AnswerText));
            }

            bool questionExists = _db.QaQuestions.Any(q => q.Id == dto.QuestionId);
            if (!questionExists)
            {
                throw new ArgumentException("Question does not exist.", nameof(dto.QuestionId));
            }

            if (dto.AnsweredById.HasValue)
            {
                bool doctorExists = _db.DoctorProfiles.Any(d => d.Id == dto.AnsweredById.Value);
                if (!doctorExists)
                {
                    throw new ArgumentException("AnsweredBy (DoctorProfile) does not exist.", nameof(dto.AnsweredById));
                }
            }

            var entity = new QaAnswer
            {
                QuestionId = dto.QuestionId,
                AnswerText = dto.AnswerText.Trim(),
                AnsweredById = dto.AnsweredById
            };

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

            if (patch.AnsweredByUserId is not null && patch.AnsweredByUserId != a.AnsweredById)
            {
                if (!_db.AppUsers.Any(u => u.Id == patch.AnsweredByUserId.Value))
                {
                    throw new ArgumentException("AnsweredBy user does not exist.");
                }

                a.AnsweredById = patch.AnsweredByUserId.Value;
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
