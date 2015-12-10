using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artportalen.Helpers
{
    using System.Text.RegularExpressions;

    public class RegexHelper
    {
        public static string GetFirstGroup(string input, string pattern)
        {
            var match = Regex.Match(input, pattern);

            return match.Groups[1].Value;
        }
    }
}
