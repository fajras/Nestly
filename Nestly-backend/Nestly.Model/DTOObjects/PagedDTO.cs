using System.Collections.Generic;

namespace Nestly.Model.DTOObjects
{
    public class PagedResult<T>
    {
        public int TotalCount { get; set; }
        public List<T> Items { get; set; } = new();
    }
}
