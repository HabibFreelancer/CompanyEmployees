using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace CompanyEmployees.IntegrationTests
{

    /*The TestAuthHandler is called to authenticate a user when the authentication scheme is set to TestScheme where AddAuthentication is registered for ConfigureTestServices.
     * It's important for the TestScheme scheme to match the scheme your app expects. Otherwise, authentication won't work*/
    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) :
            base(options, logger, encoder, clock)
        {
        }
        
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {


            var claims = new[] { new Claim(ClaimTypes.Name, "Test user"),
            new Claim(ClaimTypes.Role, "Manager") };
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "TestScheme");

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}
