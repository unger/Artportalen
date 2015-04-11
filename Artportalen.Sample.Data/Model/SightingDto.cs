namespace Artportalen.Sample.Data.Model
{
    using System;

    public class SightingDto
    {
        public long SightingId { get; set; }

        public TaxonDto Taxon { get; set; }

        public int Quantity { get; set; }

        public string Unit { get; set; }

        public SiteDto Site { get; set; }

        public int? QuantityOfSubstrate { get; set; }

        public string DiscoveryMethod { get; set; }

        public string SightingObservers { get; set; }

        public DateTime StartDate { get; set; }

        public string StartTime { get; set; }

        public DateTime EndDate { get; set; }

        public string EndTime { get; set; }

        public bool UnsureDetermination { get; set; }

        public bool NotRecovered { get; set; }

        public string PublicComment { get; set; }
    }
}
