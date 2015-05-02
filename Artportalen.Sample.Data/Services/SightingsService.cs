namespace Artportalen.Sample.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlTypes;
    using System.Linq;
    using System.Reflection;

    using Artportalen.Response;
    using Artportalen.Sample.Data.Model;

    using NHibernate.Linq;

    using NLog;

    using SafeMapper;

    public class SightingsService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private PropertyInfo[] sightingProperties;

        private PropertyInfo[] SightingProperties
        {
            get
            {
                if (this.sightingProperties == null)
                {
                    this.sightingProperties =
                        typeof(Sighting).GetProperties().Where(p => p.PropertyType == typeof(string)).ToArray();
                }

                return this.sightingProperties;
            }
        }

        public IEnumerable<SightingDto> GetSightings(DateTime date)
        {
            var nextDate = date.AddDays(1);
            using (var session = NHibernateConfiguration.GetSession())
            {
                return session.Query<SightingDto>()
                    .Where(x => x.StartDate >= date && x.EndDate < nextDate).ToArray();
            }
        }

        public long? GetLastSightingId()
        {
            using (var session = NHibernateConfiguration.GetSession())
            {
                var sighting = session.Query<SightingDto>().OrderByDescending(x => x.SightingId).FirstOrDefault();
                if (sighting != null)
                {
                    return sighting.SightingId;
                }

                return null;
            }
        }

        public void StoreSightings(IEnumerable<Sighting> sightings)
        {
            var siteDtos = new Dictionary<long, SiteDto>();
            var taxonDtos = new Dictionary<int, TaxonDto>();
            var sightingDtos = new List<SightingDto>();

            this.RemoveOldSightings();

            foreach (var sighting in sightings)
            {
                if (sighting.SightingObservers != null)
                {
                    sighting.SightingObservers = sighting.SightingObservers.Trim();
                }

                this.VerifyStringLengths(sighting);

                var taxonDto = SafeMap.Convert<Sighting, TaxonDto>(sighting);
                var siteDto = SafeMap.Convert<Sighting, SiteDto>(sighting);
                var sightingDto = SafeMap.Convert<Sighting, SightingDto>(sighting);

                if (taxonDto.TaxonId == 0)
                {
                    taxonDto.TaxonId = this.FindTaxonIdByName(taxonDto.ScientificName, taxonDto.CommonName, taxonDto.EnglishName);
                }

                // Taxons
                if (!taxonDtos.ContainsKey(taxonDto.TaxonId))
                {
                    taxonDtos.Add(taxonDto.TaxonId, taxonDto);
                }
                else
                {
                    taxonDto = taxonDtos[taxonDto.TaxonId];
                }

                sightingDto.Taxon = taxonDto;

                // Sites
                if (!siteDtos.ContainsKey(siteDto.SiteId))
                {
                    siteDtos.Add(siteDto.SiteId, siteDto);
                }
                else
                {
                    siteDto = siteDtos[siteDto.SiteId];
                }

                sightingDto.Site = siteDto;

                sightingDtos.Add(sightingDto);
            }

            using (var session = NHibernateConfiguration.GetSession())
            {
                // Taxons
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
                        if (!string.IsNullOrEmpty(taxonDto.EnglishName) && taxon.EnglishName != taxonDto.EnglishName)
                        {
                            taxon.EnglishName = taxonDto.EnglishName;
                        }

                        session.Update(taxon);
                        taxonDtos[key] = taxon;
                    }
                }

                // Sites
                foreach (var key in siteDtos.Keys.ToArray())
                {
                    var siteDto = siteDtos[key];
                    var site = session.Get<SiteDto>(key);
                    if (site == null)
                    {
                        session.Save(siteDto, siteDto.SiteId);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(siteDto.SiteName) && site.SiteName != siteDto.SiteName)
                        {
                            site.SiteName = siteDto.SiteName;
                        }

                        if (!string.IsNullOrEmpty(siteDto.Kommun) && site.Kommun != siteDto.Kommun)
                        {
                            site.Kommun = siteDto.Kommun;
                        }

                        if (!string.IsNullOrEmpty(siteDto.Landskap) && site.Landskap != siteDto.Landskap)
                        {
                            site.Landskap = siteDto.Landskap;
                        }

                        if (!string.IsNullOrEmpty(siteDto.Forsamling) && site.Forsamling != siteDto.Forsamling)
                        {
                            site.Forsamling = siteDto.Forsamling;
                        }

                        site.SiteXCoord = siteDto.SiteXCoord;
                        site.SiteYCoord = siteDto.SiteYCoord;

                        session.Update(site);

                        siteDtos[key] = site;
                    }
                }

                foreach (var sighting in sightingDtos)
                {
                    sighting.Taxon = taxonDtos[sighting.Taxon.TaxonId];
                    sighting.Site = siteDtos[sighting.Site.SiteId];

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

        private int FindTaxonIdByName(string scientificName, string commonName, string englishName)
        {
            using (var session = NHibernateConfiguration.GetSession())
            {
                var result = session.Query<TaxonInfo>().FirstOrDefault(x => x.ScientificName == scientificName || x.CommonName == commonName || x.EnglishName == englishName);

                if (result != null)
                {
                    return result.TaxonId;
                }

                return 0;
            }
        }

        private void VerifyStringLengths(Sighting sighting)
        {
            foreach (var prop in this.SightingProperties)
            {
                var value = prop.GetValue(sighting) as string;
                if (value != null)
                {
                    if (value.Length > 255 && prop.Name != "PublicComment")
                    {
                        logger.Error("SightingId: {0} String value will be truncated property: {1} value: '{2}'", sighting.SightingId, prop.Name, value.Length);
                        prop.SetValue(sighting, value.Substring(0, 254));
                    }
                }
            }
        }

        private void RemoveOldSightings()
        {
            using (var session = NHibernateConfiguration.GetSession())
            {
                session.CreateQuery("delete from SightingDto d where d.StartDate < :startDate").SetDateTime("startDate", DateTime.Today.AddDays(-3)).ExecuteUpdate();
            }
        }
    }
}
