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

        public async Task<List<QaQuestion>> Get(QaQuestionSearchObject? search)
        {
            IQueryable<QaQuestion> q = _db.QaQuestions
                .Include(x => x.AskedBy)
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

            return await q.OrderByDescending(x => x.CreatedAt).ToListAsync();
        }

        public async Task<QaQuestion?> GetById(long id) =>
            await _db.QaQuestions
                .Include(x => x.AskedBy)
                .Include(x => x.Answers)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<QaQuestion> CreateAsync(CreateQaQuestionDto dto, CancellationToken ct = default)
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
                var parentExists = await _db.ParentProfiles
                    .AnyAsync(p => p.Id == dto.AskedById.Value, ct);

                if (!parentExists)
                {
                    throw new ArgumentException("AskedBy (ParentProfile) does not exist.", nameof(dto.AskedById));
                }
            }

            var entity = new QaQuestion
            {
                QuestionText = text,
                AskedById = dto.AskedById
            };

            await _db.QaQuestions.AddAsync(entity, ct);
            await _db.SaveChangesAsync(ct);
            return entity;
        }

        public async Task<QaQuestion?> Patch(long id, QaQuestionPatchDto patch)
        {
            var q = await _db.QaQuestions.FirstOrDefaultAsync(x => x.Id == id);
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
                if (!await _db.AppUsers.AnyAsync(u => u.Id == patch.AskedByUserId.Value))
                {
                    throw new ArgumentException("AskedBy user does not exist.");
                }

                q.AskedById = patch.AskedByUserId.Value;
            }

            await _db.SaveChangesAsync();
            return q;
        }

        public async Task<bool> Delete(long id)
        {
            var q = await _db.QaQuestions.FirstOrDefaultAsync(x => x.Id == id);
            if (q is null)
            {
                return false;
            }

            _db.QaQuestions.Remove(q);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<QaAnswer> CreateAnswer(long questionId, QaAnswer answer)
        {
            var q = await _db.QaQuestions.FirstOrDefaultAsync(x => x.Id == questionId);
            if (q is null)
            {
                throw new ArgumentException("Question not found.");
            }

            if (answer.AnsweredById is not null &&
                !await _db.AppUsers.AnyAsync(u => u.Id == answer.AnsweredById.Value))
            {
                throw new ArgumentException("AnsweredBy user does not exist.");
            }

            answer.QuestionId = questionId;
            if (answer.CreatedAt == default)
            {
                answer.CreatedAt = DateTime.UtcNow;
            }

            _db.QaAnswers.Add(answer);
            await _db.SaveChangesAsync();
            return answer;
        }

        public async Task<List<QaAnswer>> GetAnswers(long questionId) =>
            await _db.QaAnswers
                .Include(a => a.AnsweredBy)
                .Where(a => a.QuestionId == questionId)
                .OrderBy(a => a.CreatedAt)
                .ToListAsync();
    }

}
