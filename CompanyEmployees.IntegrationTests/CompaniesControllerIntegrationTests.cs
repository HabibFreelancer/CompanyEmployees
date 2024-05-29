using CompanyEmployees.Extensions;
using IdentityModel;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.IntegrationTests
{
    public class CompaniesControllerIntegrationTests : IClassFixture<TestingWebAppFactory<Program>>
    {
        private readonly HttpClient _client;
        public TestServer testServer;

        /*So, we implement the TestingWebAppFactory class with the IClassFixture interface and inject it in a constructor,
         * where we create an instance of the HttpClient. The IClassFixture interface is a decorator which indicates that tests in this class rely on a fixture to run.
         * We can see that the fixture is our TestingWebAppFactory class.*/
        public CompaniesControllerIntegrationTests(TestingWebAppFactory<Program> factory)
        {
            
                _client = factory.Server.CreateClient();
                _client.SetAdminClaimsViaHeaders();

        }

        /*We Comment authorization for test*/
        [Fact]
        public async Task GetCompanies_WhenCalled_ReturnsOkObjectResult()
        {
            var response = await _client.GetAsync("/api/Companies");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Admin_Solutions Ltd", responseString);
        }

       

    }

    public static class HttpClientExtensions
    {
        public static void SetAdminClaimsViaHeaders(this HttpClient client)
        {
            var claims = new[]
            {
                new Claim(JwtClaimTypes.Subject, Guid.NewGuid().ToString()),
                new Claim(JwtClaimTypes.Name, Guid.NewGuid().ToString()),
                new Claim(JwtClaimTypes.Role,"Manager"),
            };

            var token = new JwtSecurityToken(claims: claims);
            var t = new JwtSecurityTokenHandler().WriteToken(token);
            client.DefaultRequestHeaders.Add(AuthenticatedTestRequestMiddleware.TestAuthorizationHeader, t);
        }
    }
}
