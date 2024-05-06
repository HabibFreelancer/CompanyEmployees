using CompanyEmployees.Presentation.ActionFilters;
using CompanyEmployees.Presentation.ApiBaseResponseExtensions;
using CompanyEmployees.Presentation.ModelBinders;
using Entities.Responses;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
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
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    /*this cache rule will apply to all the actions inside
the controller except the ones that already have the ResponseCache
attribute applied*/
    /*[ResponseCache(CacheProfileName = "120SecondsDuration")] => Marvin.Cache.Headers will provide for us
     * response cache attribute*/
    public class CompaniesController : ApiControllerBase
    {
        private readonly IServiceManager _service;
        public CompaniesController(IServiceManager service) => _service = service;

        /*If we inspect our CompaniesController, we can see that GetCompanies
and CreateCompany are the only actions on the root URI level
(api/companies). Therefore, we are going to create links only to them.*/
        /// <summary>
        /// Gets the list of all companies
        /// </summary>
        /// <returns>The companies list</returns>
        [HttpGet(Name = "GetCompanies")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetCompanies()
        {
            var baseResult = await _service.CompanyService.GetAllCompaniesAsync(trackChanges: false);


            /*our controller inherits from the ApiControllerBase, which inherits
from the ControllerBase class. In the GetCompanies action, we extract
the result from the service layer and cast the baseResult variable to the
concrete ApiOkResponse type, and use the Result property to extract
our required result of type IEnumerable<CompanyDto>.*/

            var companies = baseResult.GetResult<IEnumerable<CompanyDto>>(); // new ApiOkResponse<IEnumerable<CompanyDto>>((IEnumerable<CompanyDto>)baseResult).Result;

            return Ok(companies);
        }


        [HttpGet("{id:guid}", Name = "CompanyById")]
        /*Response Cache attribute => duration 60s */
        /*[ResponseCache(Duration = 60)] Marvin.Cache.Headers will provide for us
     * response cache attribute*/
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = false)]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            var baseResult = await _service.CompanyService.GetCompanyAsync(id, trackChanges:
            false);

            if (!baseResult.Success)
                return ProcessError(baseResult);

            var company = baseResult.GetResult<CompanyDto>();

            return Ok(company);
        }

        /// <summary>
        /// Creates a newly created company
        /// </summary>
        /// <param name="company"></param>
        /// <returns>A newly created company</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPost(Name = "CreateCompany")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
        {
             var createdCompany = await _service.CompanyService.CreateCompanyAsync(company);
            return CreatedAtRoute("CompanyById", new { id = createdCompany.Id },
            createdCompany);
        }

        [HttpGet("collection/({ids})", Name = "CompanyCollection")]
        public async Task<IActionResult> GetCompanyCollection
                ([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            var companies = await _service.CompanyService.GetByIdsAsync(ids, trackChanges:
            false);
            return Ok(companies);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateCompanyCollection
        ([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
        {
            var result = await
            _service.CompanyService.CreateCompanyCollectionAsync(companyCollection);
            return CreatedAtRoute("CompanyCollection", new { result.ids },
            result.companies);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            await _service.CompanyService.DeleteCompanyAsync(id, trackChanges: false);
            return NoContent();
        }


        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
        {
            await _service.CompanyService.UpdateCompanyAsync(id, company, trackChanges:
            true);
            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetCompaniesOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");
            return Ok();
        }
    }
}
