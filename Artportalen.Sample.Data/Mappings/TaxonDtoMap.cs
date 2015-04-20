namespace Artportalen.Sample.Data.Mappings
{
    using Artportalen.Sample.Data.Model;

    using FluentNHibernate.Mapping;

    public class TaxonDtoMap : ClassMap<TaxonDto>
    {
        public TaxonDtoMap()
        {
            this.Id(x => x.TaxonId).GeneratedBy.Assigned();
            this.Map(x => x.CommonName);
            this.Map(x => x.EnglishName);
            this.Map(x => x.ScientificName);
            this.Map(x => x.Author);
            this.Map(x => x.SortOrder);
            this.Map(x => x.Prefix);

            this.Version(x => x.Updated).CustomType("Timestamp").Nullable();
        }
    }
}
