using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace CompanyEmployee.BlazorUI.Common
{
    public class AcceptViaHeadersHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Accept", "text/json");

            return base.SendAsync(request, cancellationToken);
        }
    }
}
