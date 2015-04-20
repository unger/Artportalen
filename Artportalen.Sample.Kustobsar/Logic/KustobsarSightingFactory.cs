namespace Artportalen.Sample.Kustobsar.Logic
{
    using System.Globalization;
    using System.Linq;

    using Artportalen.Helpers;
    using Artportalen.Sample.Data.Model;
    using Artportalen.Sample.Kustobsar.Models;

    public class KustobsarSightingFactory
    {
        private readonly AttributeCalculator attributeCalculator;

        public KustobsarSightingFactory(AttributeCalculator attributeCalculator)
        {
            this.attributeCalculator = attributeCalculator;
        }

        public KustobsarSighting Create(SightingDto sighting)
        {
            var kustSighting = new KustobsarSighting
                                   {
                                       SightingId = sighting.SightingId,
                                       Attribute = this.attributeCalculator.GetAttribute(sighting.Quantity, sighting.StageId, sighting.GenderId, sighting.ActivityId),
                                       Quantity = sighting.Quantity.ToString(CultureInfo.InvariantCulture),
                                       StartDate = sighting.StartDate.ToString("yyyy-MM-dd"),
                                       EndDate = sighting.EndDate.ToString("yyyy-MM-dd"),
                                       StartTime = this.StripTime(sighting.StartTime),
                                       EndTime = this.StripTime(sighting.EndTime),
                                       SightingObservers = sighting.SightingObservers,
                                       PublicComment = sighting.PublicComment,
                                       RegionalStatus = "2",
                                       ReportTemplate = string.Empty,
                                       HasMedia = string.Empty,
                                   };

            if (sighting.Taxon != null)
            {
                kustSighting.TaxonId = sighting.Taxon.TaxonId;
                kustSighting.CommonName = sighting.Taxon.CommonName;
                kustSighting.ScientificName = sighting.Taxon.ScientificName;
                kustSighting.EnglishName = sighting.Taxon.EnglishName;
            }

            if (sighting.Site != null)
            {
                kustSighting.Site = this.GetSiteName(sighting.Site);
                kustSighting.SiteId = sighting.Site.SiteId.ToString(CultureInfo.InvariantCulture);
                kustSighting.SiteXCoord = sighting.Site.SiteXCoord.ToString(CultureInfo.InvariantCulture);
                kustSighting.SiteYCoord = sighting.Site.SiteXCoord.ToString(CultureInfo.InvariantCulture);
                kustSighting.RrkKod = this.GetRrkKod(sighting.Site);
            }

            return kustSighting;
        }

        private string StripTime(string startTime)
        {
            if (startTime != null && startTime.Length == 8 && startTime.EndsWith(":00"))
            {
                return startTime.Substring(0, 5);
            }

            return startTime;
        }

        private int GetRrkKod(SiteDto site)
        {
            var gbgKommuner = new[] { "Göteborg", "Mölndal", "Partille", "Härryda", "Öckerö" };

            if (site.Landskap == "Halland")
            {
                return 8;
            }

            if (gbgKommuner.Contains(site.Kommun))
            {
                return 9;
            }

            if (site.Landskap == "Bohuslän")
            {
                return 10;
            }

            return 0;
        }

        private string GetSiteName(SiteDto site)
        {
            return string.Format("{0}, {1}", site.SiteName, this.GetShortProvince(site.Landskap));
        }

        private string GetShortProvince(string landskap)
        {
            switch (landskap)
            {
                case "Västegötland":
                    return "Vg";
                case "Halland":
                    return "Hl";
                default:
                    return landskap.Substring(0, 3);
            }
        }
    }
}