namespace Artportalen.Response
{
    using System;

    public class AuthorizeToken
    {
        private DateTime createDateTime;

        public AuthorizeToken()
        {
            this.createDateTime = DateTime.Now;
        }

        public string access_token { get; set; }

        public float expires_in { get; set; }

        public DateTime AbsoluteExpiration
        {
            get
            {
                return this.createDateTime.AddSeconds(this.expires_in);
            }
        }

        public bool IsValid
        {
            get
            {

                return !string.IsNullOrEmpty(this.access_token) && DateTime.Now < this.AbsoluteExpiration;
            }
        }
    }
}
