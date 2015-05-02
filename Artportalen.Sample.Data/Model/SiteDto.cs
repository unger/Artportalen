namespace Artportalen.Sample.Data.Model
{
    using System;

    public class SiteDto
    {
        public virtual long SiteId { get; set; }

        public virtual string SiteName { get; set; }

        public virtual int Accuracy { get; set; }

        public virtual string Lan { get; set; }

        public virtual string Forsamling { get; set; }

        public virtual string Kommun { get; set; }

        public virtual string Socken { get; set; }

        public virtual string Landskap { get; set; }

        public virtual int SiteYCoord { get; set; }

        public virtual int SiteXCoord { get; set; }

        public virtual DateTime? Updated { get; set; }

        public virtual int? UseCount { get; set; }
    }
}
