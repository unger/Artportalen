using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artportalen.Tests.Response
{
    using Artportalen.Response;

    using NUnit.Framework;

    [TestFixture]
    public class PagerTests
    {
        [TestCase(10, 50, Result = 1)]
        [TestCase(50, 50, Result = 1)]
        [TestCase(60, 50, Result = 2)]
        public int PageCount(int totalCount, int pageSize)
        {
            var pager = new Pager { TotalCount = totalCount, PageSize = pageSize };

            return pager.PageCount;
        }
    }
}
