namespace Artportalen.Sample.Kustobsar.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using Artportalen.Response;
    using Artportalen.Sample.Data.Services;
    using Artportalen.Sample.Kustobsar.Logic;

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

        public ActionResult Index(string datum, string rrkkod, string sort)
        {
            DateTime date;
            if (!DateTime.TryParse(datum, out date))
            {
                date = DateTime.Today;
            }

            var sightings = this.sightingService.GetSightings(date);

            var kustobsarSightings = sightings.Select(KustobsarSightingFactory.Create);

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
    }
}