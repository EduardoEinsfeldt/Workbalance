namespace Workbalance.Hateoas
{
    public class PagedResult<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public List<Link> Links { get; set; } = new();
    }
}
