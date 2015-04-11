﻿namespace Artportalen.Sample.Data.Mappings
{
    using Artportalen.Sample.Data.Model;

    using FluentNHibernate.Mapping;

    public class SightingDtoMap : ClassMap<SightingDto>
    {
        public SightingDtoMap()
        {
            this.Id(x => x.SightingId).GeneratedBy.Assigned();
            this.Map(x => x.Quantity);
            this.Map(x => x.Unit);
            this.Map(x => x.QuantityOfSubstrate);
            this.Map(x => x.DiscoveryMethod);
            this.Map(x => x.SightingObservers);
            this.Map(x => x.StartDate);
            this.Map(x => x.StartTime);
            this.Map(x => x.EndDate);
            this.Map(x => x.EndTime);
            this.Map(x => x.UnsureDetermination);
            this.Map(x => x.NotRecovered);
            this.Map(x => x.PublicComment);

            this.References(x => x.Taxon, "TaxonId");
            this.References(x => x.Site, "SiteId");

            this.Version(x => x.Updated).CustomType("Timestamp").Nullable();
        }
    }
}
