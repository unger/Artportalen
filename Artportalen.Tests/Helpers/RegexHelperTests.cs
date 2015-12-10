using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artportalen.Tests.Helpers
{
    using Artportalen.Helpers;

    using NUnit.Framework;

    [TestFixture]
    public class RegexHelperTests
    {
        [TestCase("abc123", @"ab(c1)23", "c1")]
        [TestCase("abc123", @"([0-9]+)", "123")]
        [TestCase("abc123", @"([a-z]+)", "abc")]
        public void GetFirstGroup(string input, string pattern, string expected)
        {
            var result = RegexHelper.GetFirstGroup(input, pattern);

            Assert.AreEqual(expected, result);
        }
    }
}
