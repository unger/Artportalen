using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artportalen.Sample.Data.Mappings
{
    using Artportalen.Sample.Data.Model;

    using FluentNHibernate.Mapping;

    public class TaxonInfoMap : ClassMap<TaxonInfo>
    {
        public TaxonInfoMap()
        {
            this.Id(x => x.TaxonId).GeneratedBy.Assigned();
            this.Map(x => x.CommonName);
            this.Map(x => x.EnglishName);
            this.Map(x => x.ScientificName);
            this.Map(x => x.Auctor);
            this.Map(x => x.SortOrder);
        }
    }
}
