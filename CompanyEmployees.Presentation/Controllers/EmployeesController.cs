using Application.Commands;
using Application.Notifications;
using Application.Queries;
using CompanyEmployees.Presentation.ActionFilters;
using Entities.Models;
using Marvin.Cache.Headers;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.Controllers
{

    // TODO : apply MediatR to the employees entites 
    [ApiVersion("1.0")]
    [Route("api/companies/{companyId}/employees")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    public class EmployeesController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IPublisher _publisher;
        private readonly IServiceManager _service;
        public EmployeesController(IServiceManager service, ISender sender, IPublisher publisher)
        {
            _service = service;
            _sender = sender;
            /* inject another interface, which we are going to use to publish
                notifications.*/
            _publisher = publisher;
        }

        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        [HttpHead] /*Head HTTP Request*/
        public async Task<IActionResult> GetEmployeesForCompanyAsync(Guid companyId,
            [FromQuery] EmployeeParameters employeeParameters)
        {
            var pagedResult = await _sender.Send(new GetEmployeesForCompanyQuery(companyId, employeeParameters, trackChanges: false));
            Response.Headers.Add("X-Pagination",
                        JsonSerializer.Serialize(pagedResult.metaData));
            return Ok(pagedResult.employees);
        }

        [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
        /*Response Cache attribute => duration 60s */
        /*[ResponseCache(Duration = 60)] Marvin.Cache.Headers will provide for us
     * response cache attribute*/
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = false)]
        public async Task<IActionResult> GetEmployeeForCompanyAsync(Guid companyId, Guid id)
        {
            var employee = await _sender.Send(new GetEmployeeForCompanyByIdQuery(companyId, id, trackChanges: false));
            return Ok(employee);
        }

        /// <summary>
        /// Creates a newly created employee
        /// </summary>
        /// <param name="companyId">id company</param>
        /// <param name="employee">employee dto object</param>
        /// <returns>A newly created employee</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPost(Name = "CreateEmployeeForCompany")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateEmployeeForCompanyAsync(Guid companyId, [FromBody]
                                    EmployeeForCreationDto employee)
        {
            var employeeToReturn = await _sender.Send(new CreateEmployeeForCompanyCommand(companyId, employee, trackChanges:
            false));
            return CreatedAtRoute("GetEmployeeForCompany", new
            {
                companyId,
                id =
            employeeToReturn.Id
            },
            employeeToReturn);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEmployeeForCompanyAsync(Guid companyId, Guid id)
        {
            await _publisher.Publish(new EmployeeDeletedNotification(companyId, id, trackChanges:
            false));
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateEmployeeForCompanyAsync(Guid companyId, Guid id,
                    [FromBody] EmployeeForUpdateDto employee)
        {
            await _sender.Send(new UpdateEmployeeForCompanyCommand(companyId, id, employee,
            compTrackChanges: false, empTrackChanges: true));
            return NoContent();
        }


        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompanyAsync(Guid companyId, Guid id,
                    [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        {
            if (patchDoc is null)
                return BadRequest("patchDoc object sent from client is null.");

            await _sender.Send(new PartiallyUpdateEmployeeForCompanyCommand(patchDoc, companyId, id, compTrackChanges: false,
            empTrackChanges: true));
            return NoContent();
        }

    }

}
