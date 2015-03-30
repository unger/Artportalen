namespace Artportalen.Response
{
    public class SightingsCollection : BaseCollection<Sighting>
    {
        public Pager Pager { get; set; }
    }
}
