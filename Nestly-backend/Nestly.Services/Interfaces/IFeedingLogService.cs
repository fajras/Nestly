using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;

namespace Nestly.Services.Interfaces
{
    public interface IFeedingLogService
    {
        List<FeedingLog> Get(FeedingLogSearchObject? search);
        FeedingLog? GetById(long id);
        FeedingLog Create(CreateFeedingLogDto entity);
        FeedingLog? Patch(long id, FeedingLogPatchDto patch);
        bool Delete(long id);
    }
}
