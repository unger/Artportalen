namespace Artportalen.Request
{
    using System;

    public interface IPostSighting
    {
        int TaxonId { get; set; }

        int Quantity { get; set; }

        int ActivityId { get; set; }

        int StageId { get; set; }

        int GenderId { get; set; }

        string StartDate { get; set; }

        string EndDate { get; set; }

        bool UnsureDetermination { get; set; }

        bool NotRecovered { get; set; }

        int SiteId { get; set; }

        string SiteName { get; set; }

        string CoordNorth { get; set; }

        string CoordEast { get; set; }

        int Accuracy { get; set; }

        string PublicComment { get; set; }
    }
}