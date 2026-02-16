using Nestly.Model.DTOObjects;

public interface IFeedingLogService
{
    List<FeedingLogResponseDto> Get(FeedingLogSearchObject? search);
    FeedingLogResponseDto? GetById(long id);
    FeedingLogResponseDto Create(CreateFeedingLogDto entity);
    FeedingLogResponseDto? Patch(long id, FeedingLogPatchDto patch);
    bool Delete(long id);
}
