namespace Artportalen.Response
{
    public class CoordinateSystem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int NotationId { get; set; }

        public string NotationName { get; set; }

        public string GroupName { get; set; }

        public string GroupType { get; set; }

        public string WktFormat { get; set; }

        public string ZoneName { get; set; }
    }
}
