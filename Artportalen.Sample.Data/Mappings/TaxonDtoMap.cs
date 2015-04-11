namespace Artportalen.Sample.Data.Mappings
{
    using Artportalen.Sample.Data.Model;

    using FluentNHibernate.Mapping;

    public class TaxonDtoMap : ClassMap<TaxonDto>
    {
        public TaxonDtoMap()
        {
            this.Id(x => x.TaxonId);
            this.Map(x => x.CommonName);
            this.Map(x => x.ScientificName);
            this.Map(x => x.Author);
        }
    }
}
