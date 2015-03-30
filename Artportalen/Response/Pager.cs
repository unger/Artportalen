namespace Artportalen.Response
{
    public class Pager
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public string SortField { get; set; }

        public string SortOrder { get; set; }
    }
}