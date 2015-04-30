namespace Artportalen.Sample.Kustobsar.Logic
{
    using System;
    using System.Globalization;
    using System.Linq;

    using Artportalen.Helpers;
    using Artportalen.Sample.Data.Model;
    using Artportalen.Sample.Kustobsar.Models;

    using SwedishCoordinates;
    using SwedishCoordinates.Positions;

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
                                       ReportTemplate = string.Empty,
                                       HasMedia = string.Empty,
                                   };

            if (sighting.Taxon != null)
            {
                kustSighting.TaxonId = sighting.Taxon.TaxonId;
                kustSighting.CommonName = sighting.Taxon.CommonName;
                kustSighting.ScientificName = sighting.Taxon.ScientificName;
                kustSighting.EnglishName = sighting.Taxon.EnglishName;
                kustSighting.SortOrder = sighting.Taxon.SortOrder ?? 0;
                kustSighting.RegionalStatus = this.GetRegionalStatus(sighting.Taxon.Prefix);
            }

            if (sighting.Site != null)
            {
                var rt90 = this.GetRt90Position(sighting.Site.SiteXCoord, sighting.Site.SiteYCoord);

                kustSighting.Site = this.GetSiteName(sighting.Site);
                kustSighting.SiteId = sighting.Site.SiteId.ToString(CultureInfo.InvariantCulture);
                kustSighting.SiteXCoord = Math.Round(Math.Min(rt90.Latitude, rt90.Longitude), 0, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture);
                kustSighting.SiteYCoord = Math.Round(Math.Max(rt90.Latitude, rt90.Longitude), 0, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture);
                kustSighting.RrkKod = this.GetRrkKod(sighting.Site);
            }

            return kustSighting;
        }

        private RT90Position GetRt90Position(int siteXCoord, int siteYCoord)
        {
            var minCoord = Math.Min(siteXCoord, siteYCoord);
            var maxCoord = Math.Max(siteXCoord, siteYCoord);
            var webMerc = new WebMercatorPosition(maxCoord, minCoord);
            return PositionConverter.ToRt90(webMerc);
        }

        private string GetRegionalStatus(int? prefix)
        {
            if (!prefix.HasValue)
            {
                return "2";
            }

            switch (prefix.Value)
            {
                case 0:
                    return "8";
                case 1:
                    return "7";
                case 2:
                    return "6";
                case 3:
                    return "5";
                case 4:
                    return "4";
                case 5:
                    return "3";
                case 6:
                case 7:
                case 8:
                    return "2";
                case 9:
                    return "1";
            }

            return "2";
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

            if (site.Landskap == "Bohuslän")
            {
                if (gbgKommuner.Contains(site.Kommun))
                {
                    return 9;
                }

                return 10;
            }

            if (site.Landskap == "Västergötland" && gbgKommuner.Contains(site.Kommun))
            {
                return 9;
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
                case "Västergötland":
                    return "Vg";
                case "Halland":
                    return "Hl";
                default:
                    return landskap.Substring(0, 3);
            }
        }
    }
}