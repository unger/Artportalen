using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artportalen.Sample.Data.Model
{
    public class SiteDto
    {
        public long SiteId { get; set; }

        public string SiteName { get; set; }

        public int Accuracy { get; set; }

        public string Lan { get; set; }

        public string Forsamling { get; set; }

        public string Kommun { get; set; }

        public string Socken { get; set; }

        public string Landskap { get; set; }

        public int SiteYCoord { get; set; }

        public int SiteXCoord { get; set; }
    }
}
