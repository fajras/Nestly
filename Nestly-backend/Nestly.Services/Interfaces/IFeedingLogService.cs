using Nestly.Model.Entity;
using Nestly.Model.PatchObjects;
using Nestly.Model.SearchObjects;

namespace Nestly.Services.Interfaces
{
    public interface IFeedingLogService
    {
        List<FeedingLog> Get(FeedingLogSearchObject? search);
        FeedingLog? GetById(long id);
        FeedingLog Create(FeedingLog entity);
        FeedingLog? Patch(long id, FeedingLogPatchDto patch);
        bool Delete(long id);
    }
}
