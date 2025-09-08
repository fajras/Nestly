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

        public async Task<List<QaQuestion>> Get(QaQuestionSearchObject? search, CancellationToken ct = default)
        {
            IQueryable<QaQuestion> q = _db.QaQuestions
                .AsNoTracking()
                .Include(x => x.AskedBy)     // AppUser
                .Include(x => x.Answers);

            if (search?.AskedByUserId is not null)
            {
                q = q.Where(x => x.AskedById == search.AskedByUserId);
            }

            if (!string.IsNullOrWhiteSpace(search?.Query))
            {
                q = q.Where(x => x.QuestionText.Contains(search!.Query!));
            }

            if (search?.From is not null)
            {
                q = q.Where(x => x.CreatedAt >= search.From.Value);
            }

            if (search?.To is not null)
            {
                q = q.Where(x => x.CreatedAt <= search.To.Value);
            }

            return await q.OrderByDescending(x => x.CreatedAt).ToListAsync(ct);
        }

        public async Task<QaQuestion?> GetById(long id, CancellationToken ct = default) =>
            await _db.QaQuestions
                .AsNoTracking()
                .Include(x => x.AskedBy)     // AppUser
                .Include(x => x.Answers)
                .FirstOrDefaultAsync(x => x.Id == id, ct);

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
            var q = await _db.QaQuestions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == questionId, ct);
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

        public async Task<List<QaAnswer>> GetAnswers(long questionId, CancellationToken ct = default) =>
            await _db.QaAnswers
                .AsNoTracking()
                .Include(a => a.AnsweredBy)
                .Where(a => a.QuestionId == questionId)
                .OrderBy(a => a.CreatedAt)
                .ToListAsync(ct);
    }

}
