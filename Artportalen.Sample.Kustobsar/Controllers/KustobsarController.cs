namespace Artportalen.Sample.Kustobsar.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using Artportalen.Helpers;
    using Artportalen.Response;
    using Artportalen.Sample.Data.Services;
    using Artportalen.Sample.Kustobsar.Logic;
    using Artportalen.Sample.Kustobsar.Models;

    public class KustobsarController : Controller
    {
        private readonly SightingsService sightingService;

        private readonly KustobsarSightingFactory kustobsarSightingsFactory;

        public KustobsarController()
            : this(new SightingsService(), new KustobsarSightingFactory(new AttributeCalculator()))
        {
        }

        public KustobsarController(SightingsService sightingService, KustobsarSightingFactory kustobsarSightingsFactory)
        {
            this.sightingService = sightingService;
            this.kustobsarSightingsFactory = kustobsarSightingsFactory;
        }

        public ActionResult Index(string datum, string rrksort, string rrkkod, string sort, string sortorder)
        {
            DateTime date;
            if (!DateTime.TryParse(datum, out date))
            {
                date = DateTime.Today;
            }

            var sightings = this.sightingService.GetSightings(date);

            var kustobsarSightings = sightings.Select(this.kustobsarSightingsFactory.Create).ToList();

            kustobsarSightings.Sort(this.GetComparison(rrksort, sort, sortorder));

            return this.View(kustobsarSightings);
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

        private Comparison<KustobsarSighting> GetComparison(string rrksort, string sort, string order)
        {
            Comparison<KustobsarSighting> sortFunc;

            switch (sort)
            {
                case "2":
                    sortFunc =
                        (x, y) =>
                        order == "desc"
                            ? string.Compare(y.SiteXCoord, x.SiteXCoord, StringComparison.Ordinal)
                            : string.Compare(x.SiteXCoord, y.SiteXCoord, StringComparison.Ordinal);
                    break;
                case "4":
                    sortFunc =
                        (x, y) =>
                        order == "desc"
                            ? y.SightingId.CompareTo(x.SightingId)
                            : x.SightingId.CompareTo(y.SightingId);
                    break;
                default:
                    sortFunc =
                        (x, y) => order == "desc"
                            ? y.SortOrder.CompareTo(x.SortOrder) == 0 
                                ? y.TaxonId.CompareTo(x.TaxonId)
                                : y.SortOrder.CompareTo(x.SortOrder)
                            : x.SortOrder.CompareTo(y.SortOrder) == 0
                                ? x.TaxonId.CompareTo(y.TaxonId)
                                : x.SortOrder.CompareTo(y.SortOrder);
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
    }
}