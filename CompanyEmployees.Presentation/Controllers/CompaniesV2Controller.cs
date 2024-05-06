using CompanyEmployees.Presentation.ApiBaseResponseExtensions;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.Controllers
{
    /*By using the [ApiVersion(“2.0”)] attribute, we are stating that this
        controller is version 2.0.*/
    [ApiVersion("2.0")]
    /*if we apply the url versioning 
                                            * we can’t use the query string pattern to call the
                                            companies v2 controller anymore*/
    /*[Route("api/{v:apiversion}/companies")]*/


    [Route("api/companies")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v2")]
    public class CompaniesV2Controller : ApiControllerBase
    {
        private readonly IServiceManager _service;
        public CompaniesV2Controller(IServiceManager service) => _service = service;
        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            var baseResult = await _service.CompanyService.GetAllCompaniesAsync(trackChanges: false);
            var companies = baseResult.GetResult<IEnumerable<CompanyDto>>(); // new ApiOkResponse<IEnumerable<CompanyDto>>((IEnumerable<CompanyDto>)baseResult).Result;
            var companiesV2 = companies.Select(x => $"{x.Name} V2");
            return Ok(companiesV2);

        }
    }
}
