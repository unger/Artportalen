namespace Artportalen.Sample.Kustobsar.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    using System.Web.Mvc;

    using Artportalen.Response;
    using Artportalen.Sample.Data.Services;
    using Artportalen.Sample.Kustobsar.Logic;
    using Artportalen.Sample.Kustobsar.Models;

    public class KustobsarController : Controller
    {
        private readonly SightingsService sightingService;

        public KustobsarController()
            : this(new SightingsService())
        {
        }

        public KustobsarController(SightingsService sightingService)
        {
            this.sightingService = sightingService;
        }

        public ActionResult Index(string datum, string rrksort, string rrkkod, string sort, string sortorder)
        {
            DateTime date;
            if (!DateTime.TryParse(datum, out date))
            {
                date = DateTime.Today;
            }

            var sightings = this.sightingService.GetSightings(date);

            var kustobsarSightings = sightings.Select(KustobsarSightingFactory.Create).ToList();

            kustobsarSightings.Sort(this.GetComparison(rrksort, sort, sortorder));

            return this.View(kustobsarSightings);
        }

        private Comparison<KustobsarSighting> GetComparison(string rrksort, string sort, string sortorder)
        {
            Comparison<KustobsarSighting> sortFunc;

            switch (sort)
            {
                case "2":
                    sortFunc =
                        (x, y) =>
                        sortorder == "desc"
                            ? string.Compare(y.SiteXCoord, x.SiteXCoord, StringComparison.Ordinal)
                            : string.Compare(x.SiteXCoord, y.SiteXCoord, StringComparison.Ordinal);
                    break;
                case "4":
                    sortFunc =
                        (x, y) =>
                        sortorder == "desc"
                            ? y.SightingId.CompareTo(x.SightingId)
                            : x.SightingId.CompareTo(y.SightingId);
                    break;
                default:
                    sortFunc =
                        (x, y) => sortorder == "desc" ? y.TaxonId.CompareTo(x.TaxonId) : x.TaxonId.CompareTo(y.TaxonId);
                    break;
            }

            if (string.IsNullOrEmpty(rrksort))
            {
                return sortFunc;
            }

            return
                (x, y) =>
                rrksort == "desc"
                    ? y.RrkKod.CompareTo(x.RrkKod) == 0 ? sortFunc(x, y) : y.RrkKod.CompareTo(x.RrkKod)
                    : x.RrkKod.CompareTo(y.RrkKod) == 0 ? sortFunc(x, y) : x.RrkKod.CompareTo(y.RrkKod);
        }

        [HttpPost]
        public ActionResult Sightings(IEnumerable<Sighting> sightings)
        {
            try
            {
                if (sightings != null)
                {
                    this.sightingService.StoreSightings(sightings);
                }
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, e.Message);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}