using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;

namespace Nestly.Services.Interfaces
{
    public interface IMilestoneService
    {
        List<Milestone> Get(MilestoneSearchObject? search);
        Milestone? GetById(long id);
        Milestone Create(CreateMilestoneDto entity);
        Milestone? Patch(long id, MilestonePatchDto patch);
        bool Delete(long id);
    }
}
