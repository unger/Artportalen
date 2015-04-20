namespace Artportalen.Sample.Data.Model
{
    using System;

    public class TaxonDto
    {
        public virtual int TaxonId { get; set; }

        public virtual string CommonName { get; set; }

        public virtual string EnglishName { get; set; }

        public virtual string ScientificName { get; set; }

        public virtual string Author { get; set; }

        public virtual int? SortOrder { get; set; }

        public virtual int? Prefix { get; set; }

        public virtual DateTime? Updated { get; set; }
    }
}
