using Nestly.Model.DTOObjects;

public interface IFeedingLogService
{
    PagedResult<FeedingLogResponseDto> Get(FeedingLogSearchObject search);
    FeedingLogResponseDto GetById(long id);
    FeedingLogResponseDto Create(CreateFeedingLogDto entity);
    FeedingLogResponseDto Patch(long id, FeedingLogPatchDto patch);
    void Delete(long id);
}
