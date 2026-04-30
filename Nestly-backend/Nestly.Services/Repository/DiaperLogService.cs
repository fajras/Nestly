using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Exceptions;

public class DiaperLogService : IDiaperLogService
{
    private readonly NestlyDbContext _db;

    public DiaperLogService(NestlyDbContext db)
    {
        _db = db;
    }

    public PagedResult<DiaperLogResponseDto> Get(DiaperLogSearchObject search)
    {
        IQueryable<DiaperLog> q = _db.DiaperLogs.AsQueryable();

        if (search.BabyId is not null)
        {
            q = q.Where(x => x.BabyId == search.BabyId);
        }

        if (search.DateFrom is not null)
        {
            q = q.Where(x => x.ChangeDate >= search.DateFrom.Value);
        }

        if (search.DateTo is not null)
        {
            q = q.Where(x => x.ChangeDate <= search.DateTo.Value);
        }

        if (!string.IsNullOrWhiteSpace(search.DiaperState))
        {
            q = q.Where(x => x.DiaperState == search.DiaperState);
        }

        var totalCount = q.Count();

        var items = q
            .OrderByDescending(x => x.ChangeDate)
            .ThenByDescending(x => x.ChangeTime)
            .Skip((search.Page - 1) * search.PageSize)
            .Take(search.PageSize)
            .Select(MapToDto)
            .ToList();

        return new PagedResult<DiaperLogResponseDto>
        {
            TotalCount = totalCount,
            Items = items
        };
    }

    public DiaperLogResponseDto GetById(long id)
    {
        var entity = _db.DiaperLogs.FirstOrDefault(x => x.Id == id);

        if (entity is null)
        {
            throw new NotFoundException("Diaper log not found.");
        }

        return MapToDto(entity);
    }

    public DiaperLogResponseDto Create(CreateDiaperLogDto dto)
    {
        if (dto.BabyId <= 0)
        {
            throw new BusinessException("Baby is required.");
        }

        if (!_db.BabyProfiles.Any(b => b.Id == dto.BabyId))
        {
            throw new NotFoundException("Baby profile not found.");
        }

        if (dto.ChangeDate == default)
        {
            throw new BusinessException("Change date is required.");
        }

        if (string.IsNullOrWhiteSpace(dto.DiaperState))
        {
            throw new BusinessException("Diaper state is required.");
        }

        var entity = new DiaperLog
        {
            BabyId = dto.BabyId,
            ChangeDate = dto.ChangeDate.Date,
            ChangeTime = dto.ChangeTime,
            DiaperState = dto.DiaperState.Trim(),
            Notes = string.IsNullOrWhiteSpace(dto.Notes)
                ? null
                : dto.Notes.Trim()
        };

        _db.DiaperLogs.Add(entity);
        _db.SaveChanges();

        return MapToDto(entity);
    }
    public DiaperLogResponseDto Patch(long id, DiaperLogPatchDto patch)
    {
        var entity = _db.DiaperLogs.FirstOrDefault(x => x.Id == id);

        if (entity is null)
        {
            throw new NotFoundException("Diaper log not found.");
        }

        if (patch.ChangeDate is not null)
        {
            entity.ChangeDate = patch.ChangeDate.Value.Date;
        }

        if (patch.ChangeTime is not null)
        {
            entity.ChangeTime = patch.ChangeTime.Value;
        }

        if (patch.DiaperState is not null)
        {
            entity.DiaperState = patch.DiaperState.Trim();
        }

        if (patch.Notes is not null)
        {
            entity.Notes = string.IsNullOrWhiteSpace(patch.Notes)
                ? null
                : patch.Notes.Trim();
        }

        _db.SaveChanges();

        return MapToDto(entity);
    }

    public void Delete(long id)
    {
        var entity = _db.DiaperLogs.FirstOrDefault(x => x.Id == id);

        if (entity is null)
        {
            throw new NotFoundException("Diaper log not found.");
        }

        _db.DiaperLogs.Remove(entity);
        _db.SaveChanges();
    }

    private static DiaperLogResponseDto MapToDto(DiaperLog x)
    {
        return new DiaperLogResponseDto
        {
            Id = x.Id,
            BabyId = x.BabyId,
            ChangeDate = x.ChangeDate,
            ChangeTime = x.ChangeTime,
            DiaperState = x.DiaperState,
            Notes = x.Notes
        };
    }
}
