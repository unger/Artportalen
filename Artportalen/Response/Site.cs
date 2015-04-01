namespace Artportalen.Response
{
    public class Site
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ParentId { get; set; }

        public object ExternalId { get; set; }

        public float XCoord { get; set; }

        public float YCoord { get; set; }

        public float Distance { get; set; }

        public BaseCollection<Area> Areas { get; set; }
    }
}
