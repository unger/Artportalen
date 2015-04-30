using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artportalen.Sample.Kustobsar.Tests
{
    using Artportalen.Helpers;
    using Artportalen.Model;
    using Artportalen.Sample.Data.Model;
    using Artportalen.Sample.Kustobsar.Logic;

    using NUnit.Framework;

    using SwedishCoordinates;

    [TestFixture]
    public class KustobsarSightingFactoryTests
    {
        private KustobsarSightingFactory kustobsarSightingFactory;

        [SetUp]
        public void SetUp()
        {
            this.kustobsarSightingFactory = new KustobsarSightingFactory(new AttributeCalculator());
        }

        [TestCase(1234, Result = 1234)]
        public int TestTaxonId(int taxonId)
        {
            var sighting =
                this.kustobsarSightingFactory.Create(
                    new SightingDto
                    {
                        Taxon = new TaxonDto
                                    {
                                        TaxonId = taxonId
                                    },
                    });

            return sighting.TaxonId;
        }

        [TestCase("Skata", Result = "Skata")]
        public string TestCommonName(string commonName)
        {
            var sighting =
                this.kustobsarSightingFactory.Create(
                    new SightingDto
                    {
                        Taxon = new TaxonDto
                        {
                            CommonName = commonName
                        },
                    });

            return sighting.CommonName;
        }

        [TestCase("Pica pica", Result = "Pica pica")]
        public string TestScientificName(string scientificName)
        {
            var sighting =
                this.kustobsarSightingFactory.Create(
                    new SightingDto
                    {
                        Taxon = new TaxonDto
                        {
                            ScientificName = scientificName
                        },
                    });

            return sighting.ScientificName;
        }

        [TestCase("Magpie", Result = "Magpie")]
        public string TestEnglishName(string englishName)
        {
            var sighting =
                this.kustobsarSightingFactory.Create(
                    new SightingDto
                    {
                        Taxon = new TaxonDto
                        {
                            EnglishName = englishName
                        },
                    });

            return sighting.EnglishName;
        }

        [TestCase(1, Result = "1")]
        public string TestQuantity(int quantity)
        {
            var sighting =
                this.kustobsarSightingFactory.Create(
                    new SightingDto
                    {
                        Quantity = quantity,
                    });

            return sighting.Quantity;
        }

        [TestCase(0, null, null, null, Result = "- ex")]
        [TestCase(1, null, null, null, Result = "1 ex")]
        [TestCase(1, (int)StageEnum.Adult, null, null, Result = "1 ad")]
        public string TestAttribute(int quantity, int? stageId, int? genderId, int? activityId)
        {
            var sighting =
                this.kustobsarSightingFactory.Create(
                    new SightingDto
                    {
                        Quantity = quantity,
                        StageId = stageId,
                        GenderId = genderId,
                        ActivityId = activityId
                    });

            return sighting.Attribute;
        }

        [TestCase("13:37", Result = "13:37")]
        [TestCase("13:37:00", Result = "13:37")]
        public string TestStartTime(string startTime)
        {
            var sighting =
                this.kustobsarSightingFactory.Create(
                    new SightingDto
                    {
                        StartTime = startTime,
                    });

            return sighting.StartTime;
        }
    }
}
