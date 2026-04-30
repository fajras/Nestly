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
                Name = entity.Name
            };
        }

        public PagedResult<RoleDto> Get(RoleSearchObject search)
        {
            IQueryable<Role> query = _db.Roles.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search.Name))
            {
                query = query.Where(x => x.Name.Contains(search.Name));
            }

            var totalCount = query.Count();

            var items = query
                .OrderBy(x => x.Name)
                .Skip((search.Page - 1) * search.PageSize)
                .Take(search.PageSize)
                .Select(x => MapToDto(x))
                .ToList();

            return new PagedResult<RoleDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public RoleDto GetById(long id)
        {
            var entity = _db.Roles
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                throw new NotFoundException("Role not found.");
            }

            return MapToDto(entity);
        }

        public RoleDto Create(RoleInsertDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new BusinessException("Role name is required.");
            }

            var entity = new Role
            {
                Name = request.Name.Trim()
            };

            _db.Roles.Add(entity);
            _db.SaveChanges();

            return MapToDto(entity);
        }

        public RoleDto Update(long id, RoleUpdateDto request)
        {
            if (id == 1 || id == 2)
            {
                throw new BusinessException("System roles cannot be edited.");
            }

            var entity = _db.Roles.FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                throw new NotFoundException("Role not found.");
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                entity.Name = request.Name.Trim();
            }

            _db.SaveChanges();

            return MapToDto(entity);
        }

        public void Delete(long id)
        {
            if (id == 1 || id == 2)
            {
                throw new BusinessException("System roles cannot be deleted.");
            }

            var entity = _db.Roles.FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                throw new NotFoundException("Role not found.");
            }

            _db.Roles.Remove(entity);
            _db.SaveChanges();
        }
    }
}
