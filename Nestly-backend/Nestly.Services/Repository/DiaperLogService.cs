using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Exceptions;
using Nestly.Services.Extensions;
using Nestly.Services.Interfaces;

public class DiaperLogService : IDiaperLogService
{
    private static readonly HashSet<string> AllowedDiaperStates = new(StringComparer.OrdinalIgnoreCase)
    {
        "mokra", "stolica", "kombinovano"
    };

    private const int MinMinutesBetweenEntries = 1;

    private readonly NestlyDbContext _db;
    private readonly IBabyHealthMonitoringService _healthMonitoring;

    public DiaperLogService(NestlyDbContext db, IBabyHealthMonitoringService healthMonitoring)
    {
        _db = db;
        _healthMonitoring = healthMonitoring;
    }

    public async Task<PagedResult<DiaperLogResponseDto>> Get(DiaperLogSearchObject search)
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

        var totalCount = await q.CountAsync();
        int page = search.Page < 1 ? 1 : search.Page;

        int pageSize = search.PageSize < 1
            ? 10
            : search.PageSize > 100
                ? 100
                : search.PageSize;
        var entities = await q
            .OrderByDescending(x => x.ChangeDate)
            .ThenByDescending(x => x.ChangeTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
            .ToListAsync();

        var items = entities.Select(MapToDto).ToList();

        return new PagedResult<DiaperLogResponseDto>
        {
            TotalCount = totalCount,
            Items = items
        };
    }

    public async Task<DiaperLogResponseDto> GetById(long id)
    {
        var entity = await _db.DiaperLogs.FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
        {
            throw new NotFoundException("Diaper log not found.");
        }

        return MapToDto(entity);
    }

    public async Task<DiaperLogResponseDto> Create(CreateDiaperLogDto dto)
    {
        if (dto.BabyId <= 0)
        {
            throw new BusinessException("Baby is required.");
        }

        if (!await _db.BabyProfiles.AnyAsync(b => b.Id == dto.BabyId))
        {
            throw new NotFoundException("Baby profile not found.");
        }

        if (dto.ChangeDate == default)
        {
            throw new BusinessException("Change date is required.");
        }

        if (DateValidation.IsFutureDate(dto.ChangeDate))
        {
            throw new BusinessException("Change date cannot be in the future.");
        }

        if (string.IsNullOrWhiteSpace(dto.DiaperState))
        {
            throw new BusinessException("Diaper state is required.");
        }

        if (!AllowedDiaperStates.Contains(dto.DiaperState.Trim()))
        {
            throw new BusinessException(
                $"Diaper state must be one of: {string.Join(", ", AllowedDiaperStates)}.");
        }

        var newMoment = dto.ChangeDate.Date + dto.ChangeTime;

        var sameDayLogs = await _db.DiaperLogs
            .Where(x => x.BabyId == dto.BabyId &&
                        x.ChangeDate >= dto.ChangeDate.Date.AddDays(-1) &&
                        x.ChangeDate <= dto.ChangeDate.Date.AddDays(1))
            .Select(x => new { x.ChangeDate, x.ChangeTime })
            .ToListAsync();

        var tooClose = sameDayLogs.Any(x =>
            Math.Abs((x.ChangeDate + x.ChangeTime - newMoment).TotalMinutes) < MinMinutesBetweenEntries);

        if (tooClose)
        {
            throw new BusinessException(
                $"A diaper log already exists within {MinMinutesBetweenEntries} minute(s) of this time.");
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
        await _db.SaveChangesAsync();

        await _healthMonitoring.RunCheckForBabyAsync(dto.BabyId);

        return MapToDto(entity);
    }
    public async Task<DiaperLogResponseDto> Patch(long id, DiaperLogPatchDto patch)
    {
        var entity = await _db.DiaperLogs.FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
        {
            throw new NotFoundException("Diaper log not found.");
        }

        if (patch.ChangeDate is not null)
        {
            if (DateValidation.IsFutureDate(patch.ChangeDate.Value))
            {
                throw new BusinessException("Change date cannot be in the future.");
            }

            entity.ChangeDate = patch.ChangeDate.Value.Date;
        }

        if (patch.ChangeTime is not null)
        {
            entity.ChangeTime = patch.ChangeTime.Value;
        }

        if (patch.DiaperState is not null)
        {
            if (!AllowedDiaperStates.Contains(patch.DiaperState.Trim()))
            {
                throw new BusinessException(
                    $"Diaper state must be one of: {string.Join(", ", AllowedDiaperStates)}.");
            }

            entity.DiaperState = patch.DiaperState.Trim();
        }

        if (patch.Notes is not null)
        {
            entity.Notes = string.IsNullOrWhiteSpace(patch.Notes)
                ? null
                : patch.Notes.Trim();
        }

        await _db.SaveChangesAsync();

        return MapToDto(entity);
    }

    public async Task Delete(long id)
    {
        var entity = await _db.DiaperLogs.FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
        {
            throw new NotFoundException("Diaper log not found.");
        }

        _db.DiaperLogs.Remove(entity);
        await _db.SaveChangesAsync();
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

    public async Task<PagedResult<DiaperLogResponseDto>> GetByParent(
    long parentProfileId,
    DiaperLogSearchObject search)
    {
        IQueryable<DiaperLog> q = _db.DiaperLogs
            .Where(x =>
                x.Baby.ParentProfileId ==
                parentProfileId)
            .AsQueryable();

        if (search.BabyId is not null)
        {
            q = q.Where(x =>
                x.BabyId == search.BabyId);
        }

        if (search.DateFrom is not null)
        {
            q = q.Where(x =>
                x.ChangeDate >=
                search.DateFrom.Value);
        }

        if (search.DateTo is not null)
        {
            q = q.Where(x =>
                x.ChangeDate <=
                search.DateTo.Value);
        }

        if (!string.IsNullOrWhiteSpace(
            search.DiaperState))
        {
            q = q.Where(x =>
                x.DiaperState ==
                search.DiaperState);
        }

        var totalCount = await q.CountAsync();

        int page = search.Page < 1
            ? 1
            : search.Page;

        int pageSize = search.PageSize < 1
            ? 10
            : search.PageSize > 100
                ? 100
                : search.PageSize;

        var entities = await q
            .OrderByDescending(x => x.ChangeDate)
            .ThenByDescending(x => x.ChangeTime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var items = entities.Select(MapToDto).ToList();

        return new PagedResult<DiaperLogResponseDto>
        {
            TotalCount = totalCount,
            Items = items
        };
    }
}
