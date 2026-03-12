namespace Nestly.Model.DTOObjects
{
    public class RoleDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = default!;
    }

    public class RoleInsertDto
    {
        public string Name { get; set; } = default!;
    }

    public class RoleUpdateDto
    {
        public string Name { get; set; } = default!;
    }

    public class RoleSearchObject
    {
        public string? Name { get; set; }
    }
}
