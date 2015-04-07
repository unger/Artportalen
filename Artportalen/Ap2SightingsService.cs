namespace Artportalen
{
    using System;

    using Artportalen.Request;
    using Artportalen.Response;

    public class Ap2SightingsService
    {
        private readonly Ap2Client ap2Client;

        private readonly Ap2AuthManager authManager;

        public Ap2SightingsService(Ap2Client ap2Client, Ap2AuthManager authManager)
        {
            this.ap2Client = ap2Client;
            this.authManager = authManager;
        }

        public SightingsResponse GetTodaysSightings()
        {
            var query = new SightingsQuery
            {
                DateFrom = DateTime.Today.ToString("yyyy-MM-dd"),
                DateTo = DateTime.Today.ToString("yyyy-MM-dd"),
            };

            var response = this.ap2Client.Sightings(query, this.authManager.GetValidToken());

            return response;
        }

        public SightingsResponse GetLatestSightings()
        {
            var query = new SightingsQuery
            {
                DateFrom = DateTime.Today.AddDays(-3).ToString("yyyy-MM-dd"),
                DateTo = DateTime.Today.ToString("yyyy-MM-dd"),
                SortField = "SightingId",
            };

            var response = this.ap2Client.Sightings(query, this.authManager.GetValidToken());

            return response;
        }

        public SightingsResponse GetNextPage(SightingsResponse previousResponse)
        {
            var query = previousResponse.Query;
            query.PageNumber = previousResponse.Pager.PageIndex + 1;

            var response = this.ap2Client.Sightings(query, this.authManager.GetValidToken());

            return response;
        }
    }
}
