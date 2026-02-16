// FILE: Nestly.Services/Repository/QaQuestionService.cs
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

        public QaQuestionService(NestlyDbContext db)
        {
            _db = db;
        }

        public async Task<List<QaQuestionWithLatestAnswerDto>> GetAllWithLatestAnswer(
            QaQuestionSearchObject? search,
            CancellationToken ct = default)
        {
            IQueryable<QaQuestion> q = _db.QaQuestions.AsNoTracking();

            if (search?.AskedById is not null)
            {
                q = q.Where(x => x.AskedById == search.AskedById.Value);
            }

            if (search?.From is not null)
            {
                q = q.Where(x => x.CreatedAt >= search.From.Value);
            }

            if (search?.To is not null)
            {
                q = q.Where(x => x.CreatedAt <= search.To.Value);
            }

            if (!string.IsNullOrWhiteSpace(search?.Query))
            {
                var text = search.Query.Trim();
                q = q.Where(x => x.QuestionText.Contains(text));
            }

            if (search?.OnlyUnanswered == true)
            {
                q = q.Where(x => !x.Answers.Any());
            }

            return await q
                .Select(x => new QaQuestionWithLatestAnswerDto
                {
                    Id = x.Id,
                    QuestionText = x.QuestionText,
                    CreatedAt = x.CreatedAt,

                    IsAnswered = x.Answers.Any(),

                    LatestAnswerText = x.Answers
                        .OrderByDescending(a => a.CreatedAt)
                        .Select(a => a.AnswerText)
                        .FirstOrDefault(),

                    LatestAnswerCreatedAt = x.Answers
                        .OrderByDescending(a => a.CreatedAt)
                        .Select(a => (DateTime?)a.CreatedAt)
                        .FirstOrDefault(),

                    AnsweredByName = x.Answers
                        .OrderByDescending(a => a.CreatedAt)
                        .Select(a => a.AnsweredBy != null && a.AnsweredBy.User != null
                            ? a.AnsweredBy.User.FirstName + " " + a.AnsweredBy.User.LastName
                            : null)
                        .FirstOrDefault()
                })
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(ct);
        }

        public async Task<QaQuestionDto?> GetById(long id, CancellationToken ct = default)
        {
            return await _db.QaQuestions
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new QaQuestionDto
                {
                    Id = x.Id,
                    QuestionText = x.QuestionText,
                    AskedById = x.AskedById,
                    CreatedAt = x.CreatedAt
                })
                .FirstOrDefaultAsync(ct);
        }

        public async Task<List<QaQuestionDto>> GetByUserAsync(long askedByParentProfileId, CancellationToken ct = default)
        {
            return await _db.QaQuestions
                .AsNoTracking()
                .Where(x => x.AskedById == askedByParentProfileId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new QaQuestionDto
                {
                    Id = x.Id,
                    QuestionText = x.QuestionText,
                    AskedById = x.AskedById,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(ct);
        }

        public async Task<QaQuestionDto> Create(CreateQaQuestionDto dto, CancellationToken ct = default)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (string.IsNullOrWhiteSpace(dto.QuestionText))
            {
                throw new ArgumentException("QuestionText is required.", nameof(dto.QuestionText));
            }

            if (dto.AskedById.HasValue)
            {
                var parentExists = await _db.ParentProfiles
                    .AsNoTracking()
                    .AnyAsync(p => p.Id == dto.AskedById.Value, ct);

                if (!parentExists)
                {
                    throw new ArgumentException("AskedById (ParentProfile) does not exist.", nameof(dto.AskedById));
                }
            }

            var entity = new QaQuestion
            {
                QuestionText = dto.QuestionText.Trim(),
                AskedById = dto.AskedById,
                CreatedAt = DateTime.UtcNow
            };

            await _db.QaQuestions.AddAsync(entity, ct);
            await _db.SaveChangesAsync(ct);

            return new QaQuestionDto
            {
                Id = entity.Id,
                QuestionText = entity.QuestionText,
                AskedById = entity.AskedById,
                CreatedAt = entity.CreatedAt
            };
        }

        public async Task<QaQuestionDto?> Patch(long id, QaQuestionPatchDto patch, CancellationToken ct = default)
        {
            var q = await _db.QaQuestions.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (q is null)
            {
                return null;
            }

            if (patch is null)
            {
                throw new ArgumentNullException(nameof(patch));
            }

            if (patch.QuestionText is not null)
            {
                var text = patch.QuestionText.Trim();
                if (string.IsNullOrWhiteSpace(text))
                {
                    throw new ArgumentException("QuestionText cannot be empty.", nameof(patch.QuestionText));
                }
                q.QuestionText = text;
            }

            if (patch.AskedById is not null && patch.AskedById.Value != q.AskedById)
            {
                if (patch.AskedById.Value <= 0)
                {
                    throw new ArgumentException("AskedById must be > 0.", nameof(patch.AskedById));
                }

                var parentExists = await _db.ParentProfiles
                    .AsNoTracking()
                    .AnyAsync(p => p.Id == patch.AskedById.Value, ct);

                if (!parentExists)
                {
                    throw new ArgumentException("AskedById (ParentProfile) does not exist.", nameof(patch.AskedById));
                }

                q.AskedById = patch.AskedById.Value;
            }

            await _db.SaveChangesAsync(ct);

            return new QaQuestionDto
            {
                Id = q.Id,
                QuestionText = q.QuestionText,
                AskedById = q.AskedById,
                CreatedAt = q.CreatedAt
            };
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

        public async Task<List<QaAnswerDto>> GetAnswers(long questionId, CancellationToken ct = default)
        {
            var questionExists = await _db.QaQuestions
                .AsNoTracking()
                .AnyAsync(x => x.Id == questionId, ct);

            if (!questionExists)
            {
                throw new ArgumentException("Question not found.", nameof(questionId));
            }

            return await _db.QaAnswers
                .AsNoTracking()
                .Where(a => a.QuestionId == questionId)
                .Include(a => a.AnsweredBy)
                .ThenInclude(d => d.User)
                .OrderBy(a => a.CreatedAt)
                .Select(a => new QaAnswerDto
                {
                    Id = a.Id,
                    QuestionId = a.QuestionId,
                    AnswerText = a.AnswerText,
                    AnsweredById = a.AnsweredById,
                    CreatedAt = a.CreatedAt,
                    AnsweredByName = a.AnsweredBy != null && a.AnsweredBy.User != null
                        ? a.AnsweredBy.User.FirstName + " " + a.AnsweredBy.User.LastName
                        : null
                })
                .ToListAsync(ct);
        }

        public async Task<QaAnswerDto> CreateAnswer(long questionId, CreateQaAnswerDto dto, CancellationToken ct = default)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (string.IsNullOrWhiteSpace(dto.AnswerText))
            {
                throw new ArgumentException("AnswerText is required.", nameof(dto.AnswerText));
            }

            var qExists = await _db.QaQuestions
                .AsNoTracking()
                .AnyAsync(x => x.Id == questionId, ct);

            if (!qExists)
            {
                throw new ArgumentException("Question not found.", nameof(questionId));
            }

            if (dto.AnsweredById.HasValue)
            {
                var doctorExists = await _db.DoctorProfiles
                    .AsNoTracking()
                    .AnyAsync(d => d.Id == dto.AnsweredById.Value, ct);

                if (!doctorExists)
                {
                    throw new ArgumentException("AnsweredById (DoctorProfile) does not exist.", nameof(dto.AnsweredById));
                }
            }

            var entity = new QaAnswer
            {
                QuestionId = questionId,
                AnswerText = dto.AnswerText.Trim(),
                AnsweredById = dto.AnsweredById,
                CreatedAt = DateTime.UtcNow
            };

            await _db.QaAnswers.AddAsync(entity, ct);
            await _db.SaveChangesAsync(ct);

            var answeredByName = await _db.QaAnswers
                .AsNoTracking()
                .Where(a => a.Id == entity.Id)
                .Include(a => a.AnsweredBy)
                .ThenInclude(d => d.User)
                .Select(a => a.AnsweredBy != null && a.AnsweredBy.User != null
                    ? a.AnsweredBy.User.FirstName + " " + a.AnsweredBy.User.LastName
                    : null)
                .FirstOrDefaultAsync(ct);

            return new QaAnswerDto
            {
                Id = entity.Id,
                QuestionId = entity.QuestionId,
                AnswerText = entity.AnswerText,
                AnsweredById = entity.AnsweredById,
                CreatedAt = entity.CreatedAt,
                AnsweredByName = answeredByName
            };
        }

        public async Task<List<QaQuestionWithLatestAnswerDto>> GetWithLatestAnswerForUser(
            QaQuestionSearchObject search,
            CancellationToken ct = default)
        {
            if (search.AskedById is null || search.AskedById.Value <= 0)
            {
                throw new ArgumentException("AskedById is required.", nameof(search.AskedById));
            }

            IQueryable<QaQuestion> q = _db.QaQuestions
                .AsNoTracking()
                .Where(x => x.AskedById == search.AskedById.Value);

            if (search.From is not null)
            {
                q = q.Where(x => x.CreatedAt >= search.From.Value);
            }

            if (search.To is not null)
            {
                q = q.Where(x => x.CreatedAt <= search.To.Value);
            }

            if (!string.IsNullOrWhiteSpace(search.Query))
            {
                var text = search.Query.Trim();
                q = q.Where(x => x.QuestionText.Contains(text));
            }

            return await q
                .Select(x => new QaQuestionWithLatestAnswerDto
                {
                    Id = x.Id,
                    QuestionText = x.QuestionText,
                    CreatedAt = x.CreatedAt,

                    IsAnswered = x.Answers.Any(),

                    LatestAnswerText = x.Answers
                        .OrderByDescending(a => a.CreatedAt)
                        .Select(a => a.AnswerText)
                        .FirstOrDefault(),

                    LatestAnswerCreatedAt = x.Answers
                        .OrderByDescending(a => a.CreatedAt)
                        .Select(a => (DateTime?)a.CreatedAt)
                        .FirstOrDefault(),

                    AnsweredByName = x.Answers
                        .OrderByDescending(a => a.CreatedAt)
                        .Select(a => a.AnsweredBy != null && a.AnsweredBy.User != null
                            ? a.AnsweredBy.User.FirstName + " " + a.AnsweredBy.User.LastName
                            : null)
                        .FirstOrDefault()
                })
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(ct);
        }
    }
}
