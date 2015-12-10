namespace Artportalen.Response.Web
{
    public class SiteResponse
    {
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public string Kommun { get; set; }
        public int SiteYCoord { get; set; }
        public int SiteXCoord { get; set; }
        public int Accuracy { get; set; }
        public int ParentId { get; set; }
        public bool? IsPublic { get; set; }
    }
}
