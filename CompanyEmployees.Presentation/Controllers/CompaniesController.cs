using Application.Commands;
using Application.Notifications;
using Application.Queries;
using CompanyEmployees.Presentation.ActionFilters;
using CompanyEmployees.Presentation.ApiBaseResponseExtensions;
using CompanyEmployees.Presentation.ModelBinders;
using Entities.Responses;
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
        /// <param name="company"></param>
        /// <returns>A newly created company</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPost(Name = "CreateCompany")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        //   [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto company)
        {
            var createdCompany = await _sender.Send(new CreateCompanyCommand(company));

            return CreatedAtRoute("CompanyById", new { id = createdCompany.Id },
            createdCompany);
        }
        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDto company)
        {
            await _sender.Send(new UpdateCompanyCommand(id, company, TrackChanges: true));

            return NoContent();
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            await _publisher.Publish(new CompanyDeletedNotification(id, TrackChanges:
            false));
            return NoContent();
        }

        [HttpGet("collection/({ids})", Name = "CompanyCollection")]
        public async Task<IActionResult> GetCompanyCollection
                ([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            var companies = await _sender.Send(new GetCompaniesByIdsQuery(ids, TrackChanges: false));
            return Ok(companies);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateCompanyCollection
        ([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
        {
            var result = await _sender.Send(new CreateCompanyCollectionCommand(companyCollection));
            return CreatedAtRoute("CompanyCollection", new { result.ids },
            result.companies);
        }

        [HttpOptions]
        public IActionResult GetCompaniesOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");
            return Ok();
        }
    }
}
