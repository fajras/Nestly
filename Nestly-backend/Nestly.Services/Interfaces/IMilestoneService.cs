using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;
using Nestly.Model.SearchObjects;

namespace Nestly.Services.Interfaces
{
    public interface IMilestoneService
    {
        List<Milestone> Get(MilestoneSearchObject? search);
        Milestone? GetById(long id);
        Milestone Create(Milestone entity);
        Milestone? Patch(long id, MilestonePatchDto patch);
        bool Delete(long id);
    }
}
