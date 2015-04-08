namespace Artportalen.Response
{
    public class Pager
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public string SortField { get; set; }

        public string SortOrder { get; set; }

        public bool HasNextPage
        {
            get
            {
                return this.PageIndex < this.PageCount;
            }
        }

        public int PageCount
        {
            get
            {
                var even = this.TotalCount % this.PageSize == 0;

                return (this.TotalCount / this.PageSize) + (even ? 0 : 1);
            }
        }
    }
}