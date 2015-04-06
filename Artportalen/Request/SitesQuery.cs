namespace Artportalen.Request
{
    public class SitesQuery
    {
        public int CoordSysId { get; set; }

        public string East { get; set; }

        public string North { get; set; }

        public int Radius { get; set; }

        public int Count { get; set; }

        public int? SpeciesGroupId { get; set; }
    }
}
