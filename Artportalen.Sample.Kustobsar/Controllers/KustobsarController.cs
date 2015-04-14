namespace Artportalen.Sample.Kustobsar.Controllers
{
    using System;
    using System.Web.Mvc;

    using Artportalen.Sample.Data.Services;

    public class KustobsarController : Controller
    {
        public ActionResult Index()
        {
            var sightingService = new SightingsService();

            return this.View(sightingService.GetSightings(DateTime.Today));
        }
    }
}