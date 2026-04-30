using Nestly.Model.DTOObjects;

namespace Nestly.Services.Interfaces
{
    public interface IRoleService
    {
        PagedResult<RoleDto> Get(RoleSearchObject search);
        RoleDto? GetById(long id);
        RoleDto Create(RoleInsertDto request);
        RoleDto? Update(long id, RoleUpdateDto request);
        bool Delete(long id);
    }
}
