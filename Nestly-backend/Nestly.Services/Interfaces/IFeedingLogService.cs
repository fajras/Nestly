using Nestly.Model.DTOObjects;

public interface IFeedingLogService
{
    Task<PagedResult<FeedingLogResponseDto>> Get(FeedingLogSearchObject search);
    Task<FeedingLogResponseDto> GetById(long id);
    Task<FeedingLogResponseDto> Create(CreateFeedingLogDto entity);
    Task<FeedingLogResponseDto> Patch(long id, FeedingLogPatchDto patch);
    Task Delete(long id);
    Task<PagedResult<FeedingLogResponseDto>> GetByParent(long parentProfileId, FeedingLogSearchObject search);
}
