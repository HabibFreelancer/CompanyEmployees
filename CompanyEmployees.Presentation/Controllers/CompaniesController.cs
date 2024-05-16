using Application.Commands;
using Application.Notifications;
using Application.Queries;
using CompanyEmployees.Presentation.ActionFilters;
using CompanyEmployees.Presentation.ModelBinders;
using Marvin.Cache.Headers;
using MediatR;
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
    public class CompaniesController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IPublisher _publisher;
        public CompaniesController(ISender sender , IPublisher publisher) { 
            _sender = sender;
           /* inject another interface, which we are going to use to publish
notifications.*/
            _publisher = publisher;
        }

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
            var companies = await _sender.Send(new GetCompaniesQuery(TrackChanges: false));
            return Ok(companies);
        }

        /// <summary>
        /// Get company By Id
        /// </summary>
        /// <param name="id">The company Object</param>
        /// <returns></returns>
        [HttpGet("{id:guid}", Name = "CompanyById")]
        /*Response Cache attribute => duration 60s */
        /*[ResponseCache(Duration = 60)] Marvin.Cache.Headers will provide for us
     * response cache attribute*/
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = false)]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            var company = await _sender.Send(new GetCompanyQuery(id, TrackChanges: false));
            return Ok(company);
        }

        /// <summary>
        /// Creates a newly created company
        /// </summary>
        /// <param name="company">Company Object For Creation</param>
        /// <returns>A newly created company</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPost(Name = "CreateCompany")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
        {
            var createdCompany = await _sender.Send(new CreateCompanyCommand(company));

            return CreatedAtRoute("CompanyById", new { id = createdCompany.Id },
            createdCompany);
        }
        /// <summary>
        /// Update an existing company
        /// </summary>
        /// <param name="id">Id company to updated</param>
        /// <param name="company">Company Object to updated</param>
        /// <returns>No Content</returns>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
        {
            await _sender.Send(new UpdateCompanyCommand(id, company, TrackChanges: true));

            return NoContent();
        }

        /// <summary>
        /// Delete Existing company
        /// </summary>
        /// <param name="id">Id company to deleted</param>
        /// <returns>No Content</returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            await _publisher.Publish(new CompanyDeletedNotification(id, TrackChanges:
            false));
            return NoContent();
        }
        //Example of calling url : https://localhost:7050/api/companies/collection/(3d490a70-94ce-4d15-9494-5248280c2ce3,7a6be658-b490-4a9c-6f29-08dc61907ab2)
        /// <summary>
        /// Get collection of company by ids
        /// </summary>
        /// <param name="ids">Ids of companies to get its </param>
        /// <returns>List of the object company</returns>
        [HttpGet("collection/({ids})", Name = "CompanyCollection")]
        public async Task<IActionResult> GetCompanyCollection
                ([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            var companies = await _sender.Send(new GetCompaniesByIdsQuery(ids, TrackChanges: false));
            return Ok(companies);
        }
        /// <summary>
        /// Create a collection of company
        /// </summary>
        /// <param name="companyCollection">Collection of company to created</param>
        /// <returns>Ids created companies and list of created companies</returns>
        [HttpPost("collection")]
        public async Task<IActionResult> CreateCompanyCollection
        ([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
        {
            var result = await _sender.Send(new CreateCompanyCollectionCommand(companyCollection));
            return CreatedAtRoute("CompanyCollection", new { result.ids },
            result.companies);
        }
        /// <summary>
        /// Get the different Request Headers allowed
        /// </summary>
        /// <returns>List fo request Headers</returns>
        [HttpOptions]
        public IActionResult GetCompaniesOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");
            return Ok();
        }
    }
}
