using CompanyEmployees.IntegrationTests.Common;
using Entities.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Shared.DataTransferObjects.Company;
using Shared.DataTransferObjects.Employee;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
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

            _client = factory.Server.CreateClient();
          

        }

        /*We Comment authorization for test*/
        [Fact]
        public async Task GetCompanies_WhenCalled_ReturnsOkObjectResult()
        {
            _client.SetAdminClaimsViaHeaders();
            var response = await _client.GetAsync("/api/Companies");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Admin_Solutions Ltd", responseString);
        }


        [Fact]
        public async Task Create_SentWrongModel_ReturnsViewWithErrorMessages()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/api/Companies");

            /*[Column("CompanyId")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Company name is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Name is 60 characters.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Company address is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Address is 60 characters")]
        public string? Address { get; set; }
        public string? Country { get; set; }
        public ICollection<Employee>? Employees { get; set; }*/

            //  CompanyForCreationDto companyToPost = new CompanyForCreationDto("", "Adress 1", "Country test", new List<EmployeeForCreationDto>());

            var formModel = new Dictionary<string, string>
                        {

                             { "Name", "" },
                            { "Address", "Address Test 1" },
                                { "Country", "Contry Test 1" },
                                { "Employees", null }
                   };
            string serializeObject = JsonConvert.SerializeObject(formModel);
            postRequest.Content = new StringContent(serializeObject, Encoding.UTF8, "application/json");// new FormUrlEncodedContent(formModel);
            postRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.SendAsync(postRequest);
            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.UnprocessableEntity ,response.StatusCode );
           
            Assert.Contains("Company Name", responseString);
        }


    }
}
