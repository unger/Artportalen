namespace Artportalen.Response
{
    using System;

    public class PostSightingResponse
    {
        public bool IsSuccess { get; set; }
        public string ValidationId { get; set; }
        public Uri ValidationUrl { get; set; }
        public string Errors { get; set; }
        public PostSightingValidation SightingValidation { get; set; }
    }
}