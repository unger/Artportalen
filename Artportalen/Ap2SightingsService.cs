namespace Artportalen
{
    using System;
    using System.Linq;
    using System.Net.Http;
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

        public HttpResponseMessage LastResponseMessage
        {
            get
            {
                return this.ap2Client.LastResponseMessage;
            }
        }

        public SightingsResponse GetTodaysSightings(TaxonGroupEnum taxonGroup, long? lastSightingId = null)
        {
            return this.GetSightings((int)taxonGroup, DateTime.Today, DateTime.Today, lastSightingId);
        }

        public SightingsResponse GetYesterdaysSightings(TaxonGroupEnum taxonGroup, long? lastSightingId = null)
        {
            return this.GetSightings((int)taxonGroup, DateTime.Today.AddDays(-1), DateTime.Today.AddDays(-1), lastSightingId);
        }

        public SightingsResponse GetLastThreeDaysSightings(TaxonGroupEnum taxonGroup, long? lastSightingId = null)
        {
            return this.GetSightings((int)taxonGroup, DateTime.Today.AddDays(-2), DateTime.Today, lastSightingId);
        }

        public SightingsResponse GetNextPage(SightingsResponse previousResponse)
        {
            var query = previousResponse.Query;
            query.PageNumber = previousResponse.Pager.PageIndex + 1;

            return this.GetSightings(query);
        }

        public SightingsResponse GetSightings(int taxonId, DateTime fromDate, DateTime toDate, long? lastSightingId = null)
        {
            var query = this.GetBaseQuery(taxonId, lastSightingId);
            query.DateFrom = fromDate.ToString("yyyy-MM-dd");
            query.DateTo = toDate.ToString("yyyy-MM-dd");

            return this.GetSightings(query);
        }

        public SightingsResponse GetSightings(SightingsQuery query)
        {
            SightingsResponse result;
            try
            {
                result = this.ap2Client.Sightings(query, this.authManager.GetValidToken());
            }
            catch (UnauthorizedAccessException e)
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

        private SightingsQuery GetBaseQuery(int? taxonId, long? lastSightingId)
        {
            return new SightingsQuery
            {
                DateFrom = DateTime.Today.ToString("yyyy-MM-dd"),
                DateTo = DateTime.Today.ToString("yyyy-MM-dd"),
                SortField = "RegisterDate",
                LastSightingId = lastSightingId,
                TaxonId = taxonId,
                CoordinateSystemId = 19,
            };
        }
    }
}
