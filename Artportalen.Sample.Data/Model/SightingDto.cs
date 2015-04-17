namespace Artportalen.Sample.Data.Model
{
    using System;

    public class SightingDto
    {
        public virtual long SightingId { get; set; }

        public virtual TaxonDto Taxon { get; set; }

        public virtual int Quantity { get; set; }

        public virtual string Unit { get; set; }

        public virtual int? ActivityId { get; set; }

        public virtual int? StageId { get; set; }
        
        public virtual int? GenderId { get; set; }

        public virtual SiteDto Site { get; set; }

        public virtual int? QuantityOfSubstrate { get; set; }

        public virtual string DiscoveryMethod { get; set; }

        public virtual string SightingObservers { get; set; }

        public virtual DateTime StartDate { get; set; }

        public virtual string StartTime { get; set; }

        public virtual DateTime EndDate { get; set; }

        public virtual string EndTime { get; set; }

        public virtual bool UnsureDetermination { get; set; }

        public virtual bool NotRecovered { get; set; }

        public virtual string PublicComment { get; set; }

        public virtual DateTime? Updated { get; set; }

        public virtual string Source { get; set; }
    }
}
