﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artportalen.Sample.Kustobsar.Tests
{
    using Artportalen.Model;
    using Artportalen.Sample.Data.Model;
    using Artportalen.Sample.Kustobsar.Logic;

    using NUnit.Framework;

    [TestFixture]
    public class KustobsarSightingFactoryTests
    {
        [TestCase(1234, Result = 1234)]
        public int TestTaxonId(int taxonId)
        {
            var sighting =
                KustobsarSightingFactory.Create(
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
                KustobsarSightingFactory.Create(
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
                KustobsarSightingFactory.Create(
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
                KustobsarSightingFactory.Create(
                    new SightingDto
                    {
                        Taxon = new TaxonDto
                        {
                            EnglishName = englishName
                        },
                    });

            return sighting.EnglishName;
        }

        [TestCase(0, null, null, null, Result = "- ex")]
        [TestCase(1, null, null, null, Result = "1 ex")]
        //[TestCase(1, (int)StageEnum.Adult, null, null, Result = "1 ad")]
        public string TestAttribute(int quantity, int? stageId, int? genderId, int? activityId)
        {
            var sighting =
                KustobsarSightingFactory.Create(
                    new SightingDto
                        {
                            Quantity = quantity,
                            StageId = stageId,
                            GenderId = genderId,
                            ActivityId = activityId
                        });

            return sighting.Attribute;
        }

        [TestCase(1, Result = "1")]
        public string TestQuantity(int quantity)
        {
            var sighting =
                KustobsarSightingFactory.Create(
                    new SightingDto
                    {
                        Quantity = quantity,
                    });

            return sighting.Quantity;
        }


    }
}
