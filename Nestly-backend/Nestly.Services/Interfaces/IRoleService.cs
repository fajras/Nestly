using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IRoleService
    {
        Task<PagedResult<RoleDto>> Get(RoleSearchObject search);
        Task<RoleDto> GetById(long id);
        Task<RoleDto> Create(RoleInsertDto request);
        Task<RoleDto> Update(long id, RoleUpdateDto request);
        Task Delete(long id);
    }
}
