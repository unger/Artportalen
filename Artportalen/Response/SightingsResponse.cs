namespace Artportalen.Response
{
    using Artportalen.Request;

    public class SightingsResponse
    {
        public Pager Pager { get; set; }

        public Sighting[] Data { get; set; }

        public SightingsQuery Query { get; set; }
    }
}
