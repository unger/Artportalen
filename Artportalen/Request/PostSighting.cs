using System;

namespace Artportalen.Request
{
    public class PostSighting : IPostSighting
    {
/*        public PostSighting()
        {
            this.CoObserverIds = new int[0];
        }*/

        public int TaxonId { get; set; }
        public int Quantity { get; set; }
        public int ActivityId { get; set; }
        public int StageId { get; set; }
        public int GenderId { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public void SetStartDate(DateTime date)
        {
            var timeString = date.ToString("HH:mm:ss");
            this.StartDate = date.ToString("yyyy-MM-dd");
            this.StartTime = timeString == "00:00:00" ? null : timeString;
        }

        public void SetEndDate(DateTime date)
        {
            var timeString = date.ToString("HH:mm:ss");
            this.EndDate = date.ToString("yyyy-MM-dd");
            this.EndTime = timeString == "00:00:00" ? null : timeString;
        }

        public bool UnsureDetermination { get; set; }
        public bool NotRecovered { get; set; }

        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public string CoordNorth { get; set; }
        public string CoordEast { get; set; }
        public int Accuracy { get; set; }

        public string PublicComment { get; set; }

        public Projectvalue ProjectValue { get; set; }
        public int? CoordinateSystemId { get; set; }
        public int? CoordinateSystemNotationId { get; set; }
        public int? UnitId { get; set; }
        public int[] CoObserverIds { get; set; }
        public int? MinDepth { get; set; }
        public int? MaxDepth { get; set; }
        public int? MinHeight { get; set; }
        public int? MaxHeight { get; set; }
    }

    public class Projectvalue
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Projectparametervalue[] ProjectParameterValues { get; set; }
    }

    public class Projectparametervalue
    {
        public int ProjectParameterId { get; set; }

        public string Value { get; set; }
    }
}
