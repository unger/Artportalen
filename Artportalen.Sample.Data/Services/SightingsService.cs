namespace Artportalen.Sample.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Artportalen.Response;
    using Artportalen.Sample.Data.Model;

    using NHibernate.Linq;

    using SafeMapper;

    public class SightingsService
    {
        public void StoreSightings(IEnumerable<Sighting> sightings)
        {
            var taxonDtos = new Dictionary<int, TaxonDto>();
            var sightingDtos = new List<SightingDto>();

            foreach (var sighting in sightings)
            {
                var taxonDto = SafeMap.Convert<Sighting, TaxonDto>(sighting);
                var siteDto = SafeMap.Convert<Sighting, SiteDto>(sighting);
                var sightingDto = SafeMap.Convert<Sighting, SightingDto>(sighting);

                if (!taxonDtos.ContainsKey(taxonDto.TaxonId))
                {
                    taxonDtos.Add(taxonDto.TaxonId, taxonDto);
                }
                else
                {
                    taxonDto = taxonDtos[taxonDto.TaxonId];
                }

                sightingDto.Taxon = taxonDto;


                var site = this.GetSite(siteDto.SiteXCoord, siteDto.SiteYCoord, siteDto.SiteName);
                if (site != null)
                {
                    siteDto = site;
                }
                else
                {
                    this.CreateSite(siteDto);
                }

                sightingDto.Site = siteDto;

                sightingDtos.Add(sightingDto);
            }

            using (var session = NHibernateConfiguration.GetSession())
            {
                foreach (var key in taxonDtos.Keys.ToArray())
                {
                    var taxonDto = taxonDtos[key];
                    var taxon = session.Get<TaxonDto>(key);
                    if (taxon == null)
                    {
                        session.Save(taxonDto, taxonDto.TaxonId);
                    }
                    else
                    {
                        taxonDtos[key] = taxon;
                    }
                }

                foreach (var sighting in sightingDtos)
                {
                    sighting.Taxon = taxonDtos[sighting.Taxon.TaxonId];

                    var sightingFromDb = session.Get<SightingDto>(sighting.SightingId);
                    if (sightingFromDb == null)
                    {
                        session.Save(sighting, sighting.SightingId);
                    }
                    else
                    {
                        session.Delete(sightingFromDb);
                        session.Save(sighting, sighting.SightingId);
                    }
                }

                session.Flush();
            }
        }

        private SiteDto GetSite(int siteXCoord, int siteYCoord, string name)
        {
            using (var session = NHibernateConfiguration.GetSession())
            {
                return session.Query<SiteDto>()
                    .FirstOrDefault(x => x.SiteXCoord == siteXCoord && x.SiteYCoord == siteYCoord && x.SiteName == name);
            }
        }

        private long CreateSite(SiteDto site)
        {
            using (var session = NHibernateConfiguration.GetSession())
            {
                return (long)session.Save(site);
            }
        }

    }
}
