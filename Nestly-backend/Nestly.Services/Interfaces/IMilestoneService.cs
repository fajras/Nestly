using Nestly.Model.DTOObjects;

public interface IMilestoneService
{
    PagedResult<MilestoneResponseDto> Get(MilestoneSearchObject search);
    MilestoneResponseDto? GetById(long id);
    MilestoneResponseDto Create(CreateMilestoneDto entity);
    MilestoneResponseDto? Patch(long id, MilestonePatchDto patch);
    bool Delete(long id);
}
