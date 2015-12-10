namespace Artportalen.Response
{
    public class Site
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ParentId { get; set; }

        public object ExternalId { get; set; }

        public double XCoord { get; set; }

        public double YCoord { get; set; }

        public double Distance { get; set; }

        public BaseCollection<Area> Areas { get; set; }
    }
}
