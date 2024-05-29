using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.IntegrationTests
{
    public class CompaniesControllerIntegrationTests : IClassFixture<TestingWebAppFactory<Program>>
    {
        private readonly HttpClient _client;


        /*So, we implement the TestingWebAppFactory class with the IClassFixture interface and inject it in a constructor,
         * where we create an instance of the HttpClient. The IClassFixture interface is a decorator which indicates that tests in this class rely on a fixture to run.
         * We can see that the fixture is our TestingWebAppFactory class.*/
        public CompaniesControllerIntegrationTests(TestingWebAppFactory<Program> factory)
        {

            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });
            _client.DefaultRequestHeaders.Authorization =
                         new AuthenticationHeaderValue(scheme: "TestScheme");
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
}
