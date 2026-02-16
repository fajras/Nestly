using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;

public class DiaperLogService : IDiaperLogService
{
    private readonly NestlyDbContext _db;

    public DiaperLogService(NestlyDbContext db)
    {
        _db = db;
    }

    public IEnumerable<DiaperLogResponseDto> Get(DiaperLogSearchObject? search)
    {
        IQueryable<DiaperLog> q = _db.DiaperLogs.AsQueryable();

        if (search?.BabyId is not null)
        {
            q = q.Where(x => x.BabyId == search.BabyId);
        }

        if (search?.DateFrom is not null)
        {
            q = q.Where(x => x.ChangeDate >= search.DateFrom.Value);
        }

        if (search?.DateTo is not null)
        {
            q = q.Where(x => x.ChangeDate <= search.DateTo.Value);
        }

        if (!string.IsNullOrWhiteSpace(search?.DiaperState))
        {
            q = q.Where(x => x.DiaperState == search.DiaperState);
        }

        return q.OrderByDescending(x => x.ChangeDate)
                .ThenByDescending(x => x.ChangeTime)
                .Select(MapToDto)
                .ToList();
    }

    public DiaperLogResponseDto? GetById(long id)
    {
        var entity = _db.DiaperLogs.FirstOrDefault(x => x.Id == id);
        return entity is null ? null : MapToDto(entity);
    }

    public DiaperLogResponseDto Create(CreateDiaperLogDto dto)
    {
        if (dto.BabyId <= 0)
        {
            throw new ArgumentException("BabyId is required.");
        }

        if (dto.ChangeDate == default)
        {
            throw new ArgumentException("ChangeDate is required.");
        }

        if (string.IsNullOrWhiteSpace(dto.DiaperState))
        {
            throw new ArgumentException("DiaperState is required.");
        }

        var babyExists = _db.BabyProfiles.Any(b => b.Id == dto.BabyId);
        if (!babyExists)
        {
            throw new ArgumentException("Baby does not exist.");
        }

        var entity = new DiaperLog
        {
            BabyId = dto.BabyId,
            ChangeDate = dto.ChangeDate.Date,
            ChangeTime = dto.ChangeTime,
            DiaperState = dto.DiaperState.Trim(),
            Notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim()
        };

        _db.DiaperLogs.Add(entity);
        _db.SaveChanges();

        return MapToDto(entity);
    }

    public DiaperLogResponseDto? Patch(long id, DiaperLogPatchDto patch)
    {
        var entity = _db.DiaperLogs.FirstOrDefault(x => x.Id == id);
        if (entity is null)
        {
            return null;
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
            entity.Notes = patch.Notes.Trim();
        }

        _db.SaveChanges();

        return MapToDto(entity);
    }

    public bool Delete(long id)
    {
        var entity = _db.DiaperLogs.FirstOrDefault(x => x.Id == id);
        if (entity is null)
        {
            return false;
        }

        _db.DiaperLogs.Remove(entity);
        _db.SaveChanges();
        return true;
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
