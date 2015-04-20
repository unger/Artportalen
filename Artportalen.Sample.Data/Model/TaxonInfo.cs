namespace Artportalen.Sample.Data.Model
{
    public class TaxonInfo
    {
        public virtual int TaxonId { get; set; }

        public virtual string CommonName { get; set; }

        public virtual string EnglishName { get; set; }

        public virtual string ScientificName { get; set; }

        public virtual string Auctor { get; set; }

        public virtual int? SortOrder { get; set; }
    }
}
