namespace Artportalen.Request
{
    public class SightingsQuery
    {
        public int? SpeciesGroupId { get; set; }

        public int? TaxonId { get; set; }

        public string DateFrom { get; set; }

        public string DateTo { get; set; }

        public int? AreaDatasetId { get; set; }

        public int? FeatureId { get; set; }

        public int? SiteId { get; set; }

        public int? ProjectId { get; set; }

        public string CenterOfSearchEast { get; set; }

        public string CenterOfSearchNorth { get; set; }

        public int? CoordinateSystemId { get; set; }

        public string RadiusOfSearch { get; set; }

        public int? PageNumber { get; set; }

        public string SortField { get; set; }

        public string SortOrder { get; set; }

        public long? LastSightingId { get; set; }
    }
}
