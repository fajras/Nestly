using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Exceptions;
using Nestly.Services.Interfaces;
using Nestly.Services.Messaging;

namespace Nestly.Services.Repository
{
    public class QaQuestionService : IQaQuestionService
    {
        private readonly NestlyDbContext _db;
        private readonly RabbitMqPublisher _publisher;
        private readonly ICurrentUserService _currentUserService;
        public QaQuestionService(NestlyDbContext db, RabbitMqPublisher publisher, ICurrentUserService currentUserService)
        {
            _db = db;
            _publisher = publisher;
            _currentUserService = currentUserService;
        }

        public async Task<PagedResult<QaQuestionWithLatestAnswerDto>> GetAllWithLatestAnswer(
      QaQuestionSearchObject search,
      CancellationToken ct = default)
        {
            IQueryable<QaQuestion> q = _db.QaQuestions.AsNoTracking();

            if (search.AskedByUserId is not null)
            {
                q = q.Where(x => x.AskedBy.UserId == search.AskedByUserId.Value);
            }

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
                q = q.Where(x => x.QuestionText.Contains(search.Query.Trim()));
            }

            if (search.OnlyUnanswered == true)
            {
                q = q.Where(x => !x.Answers.Any());
            }

            var totalCount = await q.CountAsync(ct);
            int page = search.Page < 1 ? 1 : search.Page;

            int pageSize = search.PageSize < 1
                ? 10
                : search.PageSize > 100
                    ? 100
                    : search.PageSize;
            var items = await q
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
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
                .ToListAsync(ct);

            return new PagedResult<QaQuestionWithLatestAnswerDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public async Task<QaQuestionDto> GetById(long id, CancellationToken ct = default)
        {
            await _currentUserService
    .EnsureQaQuestionOwnershipAsync(id);
            var entity = await _db.QaQuestions
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

            if (entity == null)
            {
                throw new NotFoundException("Question not found.");
            }

            return entity;
        }

        public async Task<PagedResult<QaQuestionDto>> GetByUserAsync(
      long askedByParentProfileId,
      QaQuestionSearchObject search,
      CancellationToken ct = default)
        {
            var q = _db.QaQuestions
                .AsNoTracking()
                .Where(x => x.AskedById == askedByParentProfileId);

            var totalCount = await q.CountAsync(ct);
            int page = search.Page < 1 ? 1 : search.Page;

            int pageSize = search.PageSize < 1
                ? 10
                : search.PageSize > 100
                    ? 100
                    : search.PageSize;
            var items = await q
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new QaQuestionDto
                {
                    Id = x.Id,
                    QuestionText = x.QuestionText,
                    AskedById = x.AskedById,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(ct);

            return new PagedResult<QaQuestionDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public async Task<QaQuestionDto> Create(CreateQaQuestionDto dto, CancellationToken ct = default)
        {
            if (dto is null)
            {
                throw new BusinessException("Request cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(dto.QuestionText))
            {
                throw new BusinessException("Question text is required.");
            }

            var parentProfile = await _db.ParentProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    p => p.UserId == dto.CurrentUserId,
                    ct);

            if (parentProfile == null)
            {
                throw new NotFoundException("Parent profile not found.");
            }

            var entity = new QaQuestion
            {
                QuestionText = dto.QuestionText.Trim(),
                AskedById = parentProfile.Id,
                CreatedAt = DateTime.UtcNow
            };

            await _db.QaQuestions.AddAsync(entity, ct);
            await _db.SaveChangesAsync(ct);

            var doctorUserIds = await _db.DoctorProfiles
                .Select(d => d.UserId)
                .ToListAsync(ct);

            foreach (var doctorUserId in doctorUserIds)
            {
                _publisher.Publish(new NotificationEvent
                {
                    UserId = doctorUserId,
                    Title = "Novo pitanje",
                    Message = "Postavljeno je novo pitanje od strane roditelja."
                });
            }

            return new QaQuestionDto
            {
                Id = entity.Id,
                QuestionText = entity.QuestionText,
                AskedById = entity.AskedById,
                CreatedAt = entity.CreatedAt
            };
        }

        public async Task<QaQuestionDto> Patch(long id, QaQuestionPatchDto patch, CancellationToken ct = default)
        {
            await _currentUserService
    .EnsureQaQuestionOwnershipAsync(id);
            var q = await _db.QaQuestions.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (q is null)
            {
                throw new NotFoundException("Question not found.");
            }

            if (patch is null)
            {
                throw new BusinessException("Question text cannot be empty.");
            }

            if (patch.QuestionText is not null)
            {
                var text = patch.QuestionText.Trim();
                if (string.IsNullOrWhiteSpace(text))
                {
                    throw new BusinessException("Question text cannot be empty.");
                }
                q.QuestionText = text;
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

        public async Task Delete(long id, CancellationToken ct = default)
        {
            await _currentUserService
    .EnsureQaQuestionOwnershipAsync(id);
            var q = await _db.QaQuestions.FirstOrDefaultAsync(x => x.Id == id, ct);

            if (q is null)
            {
                throw new NotFoundException("Question not found.");
            }

            _db.QaQuestions.Remove(q);
            await _db.SaveChangesAsync(ct);
        }

        public async Task<PagedResult<QaAnswerDto>> GetAnswers(
     long questionId,
     QaQuestionSearchObject search,
     CancellationToken ct = default)
        {
            await _currentUserService
    .EnsureQaQuestionOwnershipAsync(questionId);
            var exists = await _db.QaQuestions.AnyAsync(x => x.Id == questionId, ct);
            if (!exists)
            {
                throw new NotFoundException("Question not found.");
            }

            var q = _db.QaAnswers
                .AsNoTracking()
                .Where(a => a.QuestionId == questionId);

            var totalCount = await q.CountAsync(ct);
            int page = search.Page < 1 ? 1 : search.Page;

            int pageSize = search.PageSize < 1
                ? 10
                : search.PageSize > 100
                    ? 100
                    : search.PageSize;
            var items = await q
                .OrderBy(a => a.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(a => a.AnsweredBy)
                .ThenInclude(d => d.User)
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

            return new PagedResult<QaAnswerDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public async Task<QaAnswerDto> CreateAnswer(
     long questionId,
     CreateQaAnswerDto dto,
     CancellationToken ct = default)
        {
            if (dto is null)
            {
                throw new BusinessException("Request cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(dto.AnswerText))
            {
                throw new BusinessException("Answer text is required.");
            }

            var qExists = await _db.QaQuestions
                .AsNoTracking()
                .AnyAsync(x => x.Id == questionId, ct);

            if (!qExists)
            {
                throw new NotFoundException("Question not found.");
            }

            var doctorProfile = await _db.DoctorProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    d => d.UserId == dto.CurrentUserId,
                    ct);

            if (doctorProfile == null)
            {
                throw new NotFoundException("Doctor profile not found.");
            }

            var entity = new QaAnswer
            {
                QuestionId = questionId,
                AnswerText = dto.AnswerText.Trim(),
                AnsweredById = doctorProfile.Id,
                CreatedAt = DateTime.UtcNow
            };

            await _db.QaAnswers.AddAsync(entity, ct);
            await _db.SaveChangesAsync(ct);

            var parentUserId = await _db.QaQuestions
                .Where(q => q.Id == questionId)
                .Select(q => q.AskedBy.UserId)
                .FirstOrDefaultAsync(ct);

            if (parentUserId > 0)
            {
                _publisher.Publish(new NotificationEvent
                {
                    UserId = parentUserId,
                    Title = "Odgovor na vaše pitanje",
                    Message = "Doktor je odgovorio na vaše pitanje."
                });
            }

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

        public async Task<PagedResult<QaQuestionWithLatestAnswerDto>> GetWithLatestAnswerForUser(
          QaQuestionSearchObject search,
          CancellationToken ct = default)
        {
            if (search.AskedByUserId is null)
            {
                throw new BusinessException("User id is required.");
            }

            var q = _db.QaQuestions
                .AsNoTracking()
                .Where(x => x.AskedBy.UserId == search.AskedByUserId.Value);

            var totalCount = await q.CountAsync(ct);
            int page = search.Page < 1 ? 1 : search.Page;

            int pageSize = search.PageSize < 1
                ? 10
                : search.PageSize > 100
                    ? 100
                    : search.PageSize;
            var items = await q
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
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
                .ToListAsync(ct);

            return new PagedResult<QaQuestionWithLatestAnswerDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }
    }
}
