using Microsoft.EntityFrameworkCore;
using Nestly.Model.DTOObjects;
using Nestly.Model.Entity;
using Nestly.Services.Data;
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

        public List<RoleDto> Get(RoleSearchObject? search)
        {
            IQueryable<Role> query = _db.Roles.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search?.Name))
            {
                query = query.Where(x => x.Name.Contains(search.Name));
            }

            return query
                .OrderBy(x => x.Name)
                .Select(x => MapToDto(x))
                .ToList();
        }

        public RoleDto? GetById(long id)
        {
            var entity = _db.Roles
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id);

            return entity == null ? null : MapToDto(entity);
        }

        public RoleDto Create(RoleInsertDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Name is required");
            }

            var entity = new Role
            {
                Name = request.Name.Trim()
            };

            _db.Roles.Add(entity);
            _db.SaveChanges();

            return MapToDto(entity);
        }

        public RoleDto? Update(long id, RoleUpdateDto request)
        {
            if (id == 1 || id == 2)
            {
                throw new Exception("System roles cannot be edited.");
            }

            var entity = _db.Roles.FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                entity.Name = request.Name.Trim();
            }

            _db.SaveChanges();

            return MapToDto(entity);
        }

        public bool Delete(long id)
        {
            if (id == 1 || id == 2)
            {
                throw new Exception("System roles cannot be deleted.");
            }

            var entity = _db.Roles.FirstOrDefault(x => x.Id == id);

            if (entity == null)
            {
                return false;
            }

            _db.Roles.Remove(entity);
            _db.SaveChanges();

            return true;
        }
    }
}
