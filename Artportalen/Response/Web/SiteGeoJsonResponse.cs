namespace Artportalen.Response.Web
{
    public class SiteGeoJsonResponse
    {
        public bool success { get; set; }
        public SitePoints points { get; set; }
        public SitePolygons polygons { get; set; }
    }

    public class SitePoints
    {
        public string type { get; set; }
        public SiteFeature[] features { get; set; }
    }

    public class SiteFeature
    {
        public string type { get; set; }
        public int id { get; set; }
        public Geometry geometry { get; set; }
        public SiteProperties properties { get; set; }
    }

    public class Geometry
    {
        public string type { get; set; }
        public int[] coordinates { get; set; }
    }

    public class SiteProperties
    {
        public string siteName { get; set; }
        public string siteAreaDescription { get; set; }
        public string siteAreaName { get; set; }
        public string projectAndExternalIdDescription { get; set; }
        public string projectAndExternalId { get; set; }
        public string coordSystemName { get; set; }
        public string accuracyDescription { get; set; }
        public int accuracy { get; set; }
        public int parentId { get; set; }
        public int siteType { get; set; }
        public int siteId { get; set; }
        public string colorString { get; set; }
        public string siteCoordinateStringPresentation { get; set; }
        public bool showAsProjectSite { get; set; }
    }

    public class SitePolygons
    {
        public string type { get; set; }
        public SiteFeature[] features { get; set; }
    }
}
