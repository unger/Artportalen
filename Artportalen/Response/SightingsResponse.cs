namespace Artportalen.Response
{
    public class SightingsResponse
    {
        public Pager Pager { get; set; }

        public Sighting[] Data { get; set; }
    }
}
