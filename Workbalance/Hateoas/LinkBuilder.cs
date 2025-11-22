using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace Workbalance.Hateoas
{
    public class LinkBuilder
    {
        private readonly IHttpContextAccessor _http;

        public LinkBuilder(IHttpContextAccessor http)
        {
            _http = http;
        }

        public Link Self(string href)
            => new Link(href, "self", "GET");

        public Link Action(string rel, string href, string method)
            => new Link(href, rel, method);

        public string GetApiVersion()
        {
            var raw = _http.HttpContext?
                .GetRequestedApiVersion()?
                .ToString();

            return raw ?? "1.0";
        }
    }
}
