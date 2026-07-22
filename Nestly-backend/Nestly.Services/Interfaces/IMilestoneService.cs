using Nestly.Model.DTOObjects;

public interface IMilestoneService
{
    Task<PagedResult<MilestoneResponseDto>> Get(MilestoneSearchObject search);
    Task<MilestoneResponseDto> GetById(long id);
    Task<MilestoneResponseDto> Create(CreateMilestoneDto entity);
    Task<MilestoneResponseDto> Patch(long id, MilestonePatchDto patch);
    Task Delete(long id);
    Task<PagedResult<MilestoneResponseDto>> GetByParent(long parentProfileId, MilestoneSearchObject search);
}
