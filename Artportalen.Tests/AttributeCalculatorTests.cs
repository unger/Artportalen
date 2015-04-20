namespace Artportalen.Tests
{
    using Artportalen.Helpers;
    using Artportalen.Model;
    using Artportalen.Response;

    using NUnit.Framework;

    [TestFixture]
    public class AttributeCalculatorTests
    {
        private AttributeCalculator attributeCalculator;

        [SetUp]
        public void SetUp()
        {
            this.attributeCalculator = new AttributeCalculator();
        }

        [TestCase(0, null, null, null, Result = "- ex")]
        [TestCase(1, null, null, null, Result = "1 ex")]
        [TestCase(1, (int)StageEnum.Adult, null, null, Result = "1 ad")]
        public string GetAttribute(int quantity, int? stageId, int? genderId, int? activityId)
        {
            return this.attributeCalculator.GetAttribute(quantity, stageId, genderId, activityId);
        }

        [TestCase("1 ex", Result = 1)]
        [TestCase("2 ex", Result = 2)]
        [TestCase("3 ad", Result = 3)]
        [TestCase("- ex", Result = 0)]
        public int GetQuantity(string attribute)
        {
            return this.attributeCalculator.GetQuantity(attribute);
        }

        [TestCase("2 ex", Result = null)]
        [TestCase("1 ad", Result = (int)StageEnum.Adult)]
        [TestCase("1 pull", Result = (int)StageEnum.Pulli)]
        public int? GetStageId(string attribute)
        {
            return this.attributeCalculator.GetStageId(attribute);
        }

        [TestCase("2 ex", Result = null)]
        [TestCase("1 hane", Result = (int)GenderEnum.Hane)]
        [TestCase("1 hona", Result = (int)GenderEnum.Hona)]
        [TestCase("1 honf", Result = (int)GenderEnum.Honfärgad)]
        [TestCase("1 hane rastande", Result = (int)GenderEnum.Hane)]
        public int? GetGenderId(string attribute)
        {
            return this.attributeCalculator.GetGenderId(attribute);
        }

        [TestCase("2 ex", Result = null)]
        [TestCase("1 ex str S", Result = (int)ActivityEnum.SträckandeS)]
        [TestCase("1 ex str SO", Result = (int)ActivityEnum.SträckandeSO)]
        [TestCase("1 hane str NO", Result = (int)ActivityEnum.SträckandeNO)]
        [TestCase("1 hane str NO", Result = (int)ActivityEnum.SträckandeNO)]
        public int? GetActivityId(string attribute)
        {
            return this.attributeCalculator.GetActivityId(attribute);
        }
    }
}
