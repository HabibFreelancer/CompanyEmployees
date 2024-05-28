using Application.Features.Company.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
    public class CompaniesV2Controller : ControllerBase
    {
        private readonly ISender _sender;
        public CompaniesV2Controller(ISender sender) => _sender = sender;
        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _sender.Send(new GetCompaniesQuery(TrackChanges: false));
            var companiesV2 = companies.Select(x => $"{x.Name} V2");
            return Ok(companiesV2);

        }
    }
}
