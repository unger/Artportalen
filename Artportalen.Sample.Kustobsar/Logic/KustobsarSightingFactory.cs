namespace Artportalen.Sample.Kustobsar.Logic
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;

    using Artportalen.Model;
    using Artportalen.Sample.Data.Model;
    using Artportalen.Sample.Kustobsar.Models;

    using SafeMapper;

    public class KustobsarSightingFactory
    {
        public static KustobsarSighting Create(SightingDto sighting)
        {
            var kustSighting = new KustobsarSighting
                                   {
                                       SightingId = sighting.SightingId,
                                       Attribute = GetAttribute(sighting),
                                       Quantity = sighting.Quantity.ToString(CultureInfo.InvariantCulture),
                                       StartDate = sighting.StartDate.ToString("yyyy-MM-dd"),
                                       EndDate = sighting.EndDate.ToString("yyyy-MM-dd"),
                                       StartTime = StripTime(sighting.StartTime),
                                       EndTime = StripTime(sighting.EndTime),
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
                kustSighting.Site = GetSiteName(sighting.Site);
                kustSighting.SiteId = sighting.Site.SiteId.ToString(CultureInfo.InvariantCulture);
                kustSighting.SiteXCoord = sighting.Site.SiteXCoord.ToString(CultureInfo.InvariantCulture);
                kustSighting.SiteYCoord = sighting.Site.SiteXCoord.ToString(CultureInfo.InvariantCulture);
                kustSighting.RrkKod = GetRrkKod(sighting.Site);
            }


            return kustSighting;
        }

        private static string StripTime(string startTime)
        {
            if (startTime != null && startTime.Length == 8 && startTime.EndsWith(":00"))
            {
                return startTime.Substring(0, 5);
            }

            return startTime;
        }

        private static int GetRrkKod(SiteDto site)
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

        private static string GetSiteName(SiteDto site)
        {
            return string.Format("{0}, {1}", site.SiteName, GetShortProvince(site.Landskap));
        }

        private static string GetShortProvince(string landskap)
        {
            switch (landskap)
            {
                case "Västegötland":
                    return "vg";
                case "Halland":
                    return "hl";
                default:
                    return landskap.Substring(0, 3).ToLower();
            }
        }

        private static string GetAttribute(SightingDto sighting)
        {
            var attributes = new List<string>();
            var stage = SafeMap.Convert<int, StageEnum>(sighting.StageId ?? 0);
            var gender = SafeMap.Convert<int, GenderEnum>(sighting.GenderId ?? 0);
            var activity = SafeMap.Convert<int, ActivityEnum>(sighting.ActivityId ?? 0);

            attributes.Add(sighting.Quantity == 0 ? "-" : sighting.Quantity.ToString(CultureInfo.InvariantCulture));

            if (gender == GenderEnum.Undefined && stage == StageEnum.Undefined)
            {
                attributes.Add("ex");
            }
            else
            {
                if (stage != StageEnum.Undefined)
                {
                    attributes.Add(GetEnumDisplayShortName(stage));
                }

                if (gender != GenderEnum.Undefined)
                {
                    attributes.Add(GetEnumDisplayShortName(gender));
                }
            }

            if (activity != ActivityEnum.Undefined)
            {
                attributes.Add(GetEnumDisplayShortName(activity));
            }

            return string.Join(" ", attributes);
        }

        private static string GetEnumDisplayShortName(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            var attributes =
                (DisplayAttribute[])fi.GetCustomAttributes(
                typeof(DisplayAttribute),
                false);

            if (attributes.Length > 0)
            {
                return attributes[0].GetShortName();
            }

            return value.ToString();
        }
    }
}