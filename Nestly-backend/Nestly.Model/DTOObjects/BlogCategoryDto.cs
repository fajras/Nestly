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

        public int Page { get; set; } = 1;

        private int _pageSize = 20;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > 100 ? 100 : value;
        }
    }
}
