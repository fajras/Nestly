using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
using Nestly.Services.Exceptions;
using Nestly.Services.Interfaces;
namespace Nestly.Services.Repository
{
    public class RoleService : IRoleService
    {
        private readonly NestlyDbContext _db;

        public RoleService(NestlyDbContext db)
        {
            _db = db;
        }

        private static RoleDto MapToDto(Role entity)
        {
            return new RoleDto
            {
                Id = entity.Id,
                Name = entity.Name,
                IsSystemRole = entity.IsSystemRole
            };
        }

        public async Task<PagedResult<RoleDto>> Get(RoleSearchObject search)
        {
            IQueryable<Role> query = _db.Roles.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search.Name))
            {
                query = query.Where(x => x.Name.Contains(search.Name));
            }

            var totalCount = await query.CountAsync();
            int page = search.Page < 1 ? 1 : search.Page;

            int pageSize = search.PageSize < 1
                ? 10
                : search.PageSize > 100
                    ? 100
                    : search.PageSize;
            var entities = await query
                .OrderBy(x => x.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = entities.Select(x => MapToDto(x)).ToList();

            return new PagedResult<RoleDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public async Task<RoleDto> GetById(long id)
        {
            var entity = await _db.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                throw new NotFoundException("Role not found.");
            }

            return MapToDto(entity);
        }

        public async Task<RoleDto> Create(RoleInsertDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new BusinessException("Role name is required.");
            }

            var name = request.Name.Trim();

            bool duplicate = await _db.Roles
                .AnyAsync(x => x.Name.ToLower() == name.ToLower());

            if (duplicate)
            {
                throw new BusinessException("A role with this name already exists.");
            }

            var entity = new Role
            {
                Name = name,
                IsSystemRole = false
            };

            _db.Roles.Add(entity);
            await _db.SaveChangesAsync();

            return MapToDto(entity);
        }

        public async Task<RoleDto> Update(long id, RoleUpdateDto request)
        {
            var entity = await _db.Roles.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                throw new NotFoundException("Role not found.");
            }

            if (entity.IsSystemRole)
            {
                throw new BusinessException("System roles cannot be edited.");
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var name = request.Name.Trim();

                bool duplicate = await _db.Roles
                    .AnyAsync(x => x.Id != id && x.Name.ToLower() == name.ToLower());

                if (duplicate)
                {
                    throw new BusinessException("A role with this name already exists.");
                }

                entity.Name = name;
            }

            await _db.SaveChangesAsync();

            return MapToDto(entity);
        }

        public async Task Delete(long id)
        {
            var entity = await _db.Roles.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                throw new NotFoundException("Role not found.");
            }

            if (entity.IsSystemRole)
            {
                throw new BusinessException("System roles cannot be deleted.");
            }

            _db.Roles.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }
}
