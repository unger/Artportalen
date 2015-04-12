namespace Artportalen
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Security.Authentication;

    using Artportalen.Model;
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

        public SightingsResponse GetTodaysSightings(SpeciesGroupEnum speciesGroup, long? lastSightingId = null)
        {
            var query = this.GetBaseQuery(speciesGroup, lastSightingId);

            return this.GetSightingsResponse(query);
        }

        public SightingsResponse GetYesterdaysSightings(SpeciesGroupEnum speciesGroup, long? lastSightingId = null)
        {
            var query = this.GetBaseQuery(speciesGroup, lastSightingId);
            query.DateFrom = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
            query.DateTo = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");

            return this.GetSightingsResponse(query);
        }

        public SightingsResponse GetLastThreeDaysSightings(SpeciesGroupEnum speciesGroup, long? lastSightingId = null)
        {
            var query = this.GetBaseQuery(speciesGroup, lastSightingId);
            query.DateFrom = DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd");

            return this.GetSightingsResponse(query);
        }

        public SightingsResponse GetNextPage(SightingsResponse previousResponse)
        {
            var query = previousResponse.Query;
            query.PageNumber = previousResponse.Pager.PageIndex + 1;

            return this.GetSightingsResponse(query);
        }

        private SightingsResponse GetSightingsResponse(SightingsQuery query)
        {
            SightingsResponse result;
            try
            {
                result = this.ap2Client.Sightings(query, this.authManager.GetValidToken());
            }
            catch (AuthenticationException e)
            {
                result = this.ap2Client.Sightings(query, this.authManager.GetNewToken());
            }

            if (query.LastSightingId.HasValue && query.LastSightingId > 0)
            {
                var newData = result.Data.Where(s => s.SightingId > query.LastSightingId).ToArray();
                if (result.Data.Length > newData.Length)
                {
                    result.Data = newData;
                    result.Pager.TotalCount = ((result.Pager.PageIndex - 1) * result.Pager.PageSize) + newData.Length;
                }
            }

            return result;
        }

        private SightingsQuery GetBaseQuery(SpeciesGroupEnum speciesGroup, long? lastSightingId)
        {
            return new SightingsQuery
            {
                DateFrom = DateTime.Today.ToString("yyyy-MM-dd"),
                DateTo = DateTime.Today.ToString("yyyy-MM-dd"),
                SortField = "SightingId",
                LastSightingId = lastSightingId,
                SpeciesGroupId = (int)speciesGroup,
            };
        }
    }
}
