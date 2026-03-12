namespace Nestly.Model.DTOObjects
{
    public class BlogCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
    }

    public class BlogCategoryInsertDto
    {
        public string Name { get; set; } = default!;
    }

    public class BlogCategoryUpdateDto
    {
        public string Name { get; set; } = default!;
    }

    public class BlogCategorySearchObject
    {
        public string? Name { get; set; }
    }
}
