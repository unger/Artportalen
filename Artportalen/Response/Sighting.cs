namespace Artportalen.Response
{
    using System;

    public class Sighting
    {
        public int SightingId { get; set; }

        public string ScientificName { get; set; }

        public string Author { get; set; }

        public string CommonName { get; set; }

        public string Lan { get; set; }

        public string Forsamling { get; set; }

        public string Kommun { get; set; }

        public string Socken { get; set; }

        public string Landskap { get; set; }

        public int SiteYCoord { get; set; }

        public int SiteXCoord { get; set; }

        public string Unit { get; set; }

        public int QuantityOfSubstrate { get; set; }

        public string DiscoveryMethod { get; set; }

        public int Length { get; set; }

        public int Weight { get; set; }

        public PrivateComment PrivateComment { get; set; }

        public string SightingObservers { get; set; }

        public DateTime StartDate { get; set; }

        public string StartTime { get; set; }

        public DateTime EndDate { get; set; }

        public string EndTime { get; set; }

        public int TaxonId { get; set; }

        public int Quantity { get; set; }

        public string SiteName { get; set; }

        public int Accuracy { get; set; }

        public bool UnsureDetermination { get; set; }

        public bool NotRecovered { get; set; }

        public int MinDepth { get; set; }

        public int MaxDepth { get; set; }

        public int MinHeight { get; set; }

        public int MaxHeight { get; set; }

        public string PublicComment { get; set; }
    }
}
