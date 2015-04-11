namespace Artportalen.Sample.Data.Mappings
{
    using Artportalen.Sample.Data.Model;

    using FluentNHibernate.Mapping;

    public class SiteDtoMap : ClassMap<SiteDto>
    {
        public SiteDtoMap()
        {
            this.Id(x => x.SiteId);
            this.Map(x => x.SiteName);
            this.Map(x => x.Accuracy);
            this.Map(x => x.Lan);
            this.Map(x => x.Forsamling);
            this.Map(x => x.Kommun);
            this.Map(x => x.Socken);
            this.Map(x => x.Landskap);
            this.Map(x => x.SiteYCoord);
            this.Map(x => x.SiteXCoord);
        }
    }
}
