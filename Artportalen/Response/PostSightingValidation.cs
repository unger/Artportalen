namespace Artportalen.Response
{
    public class PostSightingValidation
    {
        public int NumberOfErrors { get; set; }
        public string StartDateTimeError { get; set; }
        public string EndDateTimeError { get; set; }
        public string TaxonError { get; set; }
        public string QuantityError { get; set; }
        public string CoordinateSystemError { get; set; }
        public string SiteError { get; set; }
        public string UnitError { get; set; }
        public string StageError { get; set; }
        public string GenderError { get; set; }
        public string ActivityError { get; set; }
        public string ObserversError { get; set; }
        public string ProjectError { get; set; }
        public string DepthHeightError { get; set; }
        public string DataFormatError { get; set; }
    }

}
