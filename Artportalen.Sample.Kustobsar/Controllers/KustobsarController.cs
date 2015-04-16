namespace Artportalen.Sample.Kustobsar.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Web.Mvc;

    using Artportalen.Response;
    using Artportalen.Sample.Data.Services;

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

        public ActionResult Index()
        {
            return this.View(this.sightingService.GetSightings(DateTime.Today));
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