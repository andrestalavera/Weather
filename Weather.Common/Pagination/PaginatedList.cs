using System.Collections.Generic;

namespace Weather.Common.Pagination
{
    public class PaginatedList<T>
        where T : class
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public int Total { get; set; }
        public int Pages { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
