using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Interfaces;

namespace Nestly.Services.Repository
{
    public class QaQuestionService : IQaQuestionService
    {
        private readonly NestlyDbContext _db;
        public QaQuestionService(NestlyDbContext db) => _db = db;

        public async Task<List<QaQuestionWithLatestAnswerDto>> GetAllWithLatestAnswer(
     QaQuestionSearchObject? search,
     CancellationToken ct = default)
        {
            var query = _db.QaQuestions
                .AsNoTracking()
                .Include(q => q.Answers)
                    .ThenInclude(a => a.AnsweredBy)
                        .ThenInclude(u => u.User)
                .AsQueryable();
            query = query.Where(q => !q.Answers.Any());
            if (search?.From is not null)
            {
                query = query.Where(q => q.CreatedAt >= search.From.Value);
            }

            if (search?.To is not null)
            {
                query = query.Where(q => q.CreatedAt <= search.To.Value);
            }

            if (!string.IsNullOrWhiteSpace(search?.Query))
            {
                query = query.Where(q => q.QuestionText.Contains(search.Query));
            }

            return await query
                .Select(q => new QaQuestionWithLatestAnswerDto
                {
                    Id = q.Id,
                    QuestionText = q.QuestionText,
                    CreatedAt = q.CreatedAt,

                    IsAnswered = q.Answers.Any(),

                    LatestAnswerText = q.Answers
                        .OrderByDescending(a => a.CreatedAt)
                        .Select(a => a.AnswerText)
                        .FirstOrDefault(),

                    LatestAnswerCreatedAt = q.Answers
                        .OrderByDescending(a => a.CreatedAt)
                        .Select(a => (DateTime?)a.CreatedAt)
                        .FirstOrDefault(),

                    AnsweredByName = q.Answers
                        .OrderByDescending(a => a.CreatedAt)
                        .Select(a => a.AnsweredBy != null
                            ? a.AnsweredBy.User.FirstName + " " + a.AnsweredBy.User.LastName
                            : null)
                        .FirstOrDefault()
                })
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(ct);
        }


        public async Task<QaQuestion?> GetById(long id, CancellationToken ct = default) =>
            await _db.QaQuestions
                .AsNoTracking()
                .Include(x => x.AskedBy)
                .Include(x => x.Answers)
                .FirstOrDefaultAsync(x => x.Id == id, ct);

        public async Task<List<QaQuestion>> GetByUserAsync(long askedByUserId, CancellationToken ct = default)
        {
            return await _db.QaQuestions
                .AsNoTracking()
                .Include(x => x.AskedBy)
                .Include(x => x.Answers)
                .Where(x => x.AskedById == askedByUserId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(ct);
        }

        public async Task<QaQuestion> Create(CreateQaQuestionDto dto, CancellationToken ct = default)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (string.IsNullOrWhiteSpace(dto.QuestionText))
            {
                throw new ArgumentException("QuestionText is required.", nameof(dto.QuestionText));
            }

            var text = dto.QuestionText.Trim();

            if (dto.AskedById.HasValue)
            {
                var userExists = await _db.AppUsers
                    .AsNoTracking()
                    .AnyAsync(u => u.Id == dto.AskedById.Value, ct);

                if (!userExists)
                {
                    throw new ArgumentException("AskedBy user does not exist.", nameof(dto.AskedById));
                }
            }

            var entity = new QaQuestion
            {
                QuestionText = text,
                AskedById = dto.AskedById,
                CreatedAt = DateTime.UtcNow
            };

            await _db.QaQuestions.AddAsync(entity, ct);
            await _db.SaveChangesAsync(ct);

            return entity;
        }

        public async Task<QaQuestion?> Patch(long id, QaQuestionPatchDto patch, CancellationToken ct = default)
        {
            var q = await _db.QaQuestions.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (q is null)
            {
                return null;
            }

            if (patch.QuestionText is not null)
            {
                q.QuestionText = patch.QuestionText;
            }

            if (patch.AskedByUserId is not null && patch.AskedByUserId != q.AskedById)
            {
                var exists = await _db.AppUsers
                    .AsNoTracking()
                    .AnyAsync(u => u.Id == patch.AskedByUserId.Value, ct);

                if (!exists)
                {
                    throw new ArgumentException("AskedBy user does not exist.", nameof(patch.AskedByUserId));
                }

                q.AskedById = patch.AskedByUserId.Value;
            }

            await _db.SaveChangesAsync(ct);
            return q;
        }

        public async Task<bool> Delete(long id, CancellationToken ct = default)
        {
            var q = await _db.QaQuestions.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (q is null)
            {
                return false;
            }

            _db.QaQuestions.Remove(q);
            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<QaAnswer> CreateAnswer(long questionId, QaAnswer answer, CancellationToken ct = default)
        {
            var q = await _db.QaQuestions
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == questionId, ct);

            if (q is null)
            {
                throw new ArgumentException("Question not found.", nameof(questionId));
            }

            if (answer.AnsweredById is not null)
            {
                var userExists = await _db.AppUsers
                    .AsNoTracking()
                    .AnyAsync(u => u.Id == answer.AnsweredById.Value, ct);

                if (!userExists)
                {
                    throw new ArgumentException("AnsweredBy user does not exist.", nameof(answer.AnsweredById));
                }
            }

            answer.QuestionId = questionId;

            if (answer.CreatedAt == default)
            {
                answer.CreatedAt = DateTime.UtcNow;
            }

            _db.QaAnswers.Add(answer);
            await _db.SaveChangesAsync(ct);

            return answer;
        }

        public async Task<List<QaAnswer>> GetAnswers(long questionId, CancellationToken ct = default)
        {
            return await _db.QaAnswers
                .AsNoTracking()
                .Include(a => a.AnsweredBy)
                .Where(a => a.QuestionId == questionId)
                .OrderBy(a => a.CreatedAt)
                .ToListAsync(ct);
        }

        public async Task<List<QaQuestionWithLatestAnswerDto>> GetWithLatestAnswer(
    QaQuestionSearchObject search,
    CancellationToken ct = default)
        {
            var query = _db.QaQuestions
                .AsNoTracking()
                .Where(q => q.AskedById == search.AskedByUserId);

            return await query
                .Select(q => new QaQuestionWithLatestAnswerDto
                {
                    Id = q.Id,
                    QuestionText = q.QuestionText,
                    CreatedAt = q.CreatedAt,

                    IsAnswered = q.Answers.Any(),

                    LatestAnswerText = q.Answers
                        .OrderByDescending(a => a.CreatedAt)
                        .Select(a => a.AnswerText)
                        .FirstOrDefault(),

                    LatestAnswerCreatedAt = q.Answers
                        .OrderByDescending(a => a.CreatedAt)
                        .Select(a => (DateTime?)a.CreatedAt)
                        .FirstOrDefault(),

                    AnsweredByName = q.Answers
                        .OrderByDescending(a => a.CreatedAt)
                        .Select(a => a.AnsweredBy != null
                            ? a.AnsweredBy.User.FirstName + " " + a.AnsweredBy.User.LastName
                            : null)
                        .FirstOrDefault()
                })
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(ct);
        }

    }
}
