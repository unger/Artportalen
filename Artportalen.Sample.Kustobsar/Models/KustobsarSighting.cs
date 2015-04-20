namespace Artportalen.Sample.Kustobsar.Models
{
    public class KustobsarSighting
    {
        public long SightingId { get; set; }

        public int TaxonId { get; set; }

        public string CommonName { get; set; }

        public string ScientificName { get; set; }

        public string EnglishName { get; set; }

        public string Attribute { get; set; }

        public string Quantity { get; set; }

        public string Site { get; set; }

        public string SiteId { get; set; }

        public string SiteXCoord { get; set; }

        public string SiteYCoord { get; set; }

        public int RrkKod { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string SightingObservers { get; set; }

        public string PublicComment { get; set; }

        public string RegionalStatus { get; set; }

        public string ReportTemplate { get; set; }

        public string HasMedia { get; set; }

        public int? SortOrder { get; set; }
    }
}