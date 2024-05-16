using Application.Commands;
using Application.Notifications;
using Application.Queries;
using CompanyEmployees.Presentation.ActionFilters;
using Entities.Models;
using Marvin.Cache.Headers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Data;
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
    /*this cache rule will apply to all the actions inside
the controller except the ones that already have the ResponseCache
attribute applied*/
    /*[ResponseCache(CacheProfileName = "120SecondsDuration")] => Marvin.Cache.Headers will provide for us
     * response cache attribute*/

    public class EmployeesController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IPublisher _publisher;
        public EmployeesController( ISender sender, IPublisher publisher)
        {
            _sender = sender;
            /* inject another interface, which we are going to use to publish
                notifications.*/
            _publisher = publisher;
        }
        /// <summary>
        /// Gets the list of all employee by company
        /// </summary>
        /// <param name="companyId">Id company</param>
        /// <param name="employeeParameters"> Filter parameters (max Age, min Age , search term ....)</param>
        /// <returns>The employee list with paging</returns>
        [HttpHead] /*Head HTTP Request*/
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetEmployeesForCompanyAsync(Guid companyId,
            [FromQuery] EmployeeParameters employeeParameters)
        {
            var pagedResult = await _sender.Send(new GetEmployeesForCompanyQuery(companyId, employeeParameters, trackChanges: false));
            Response.Headers.Add("X-Pagination",
                        JsonSerializer.Serialize(pagedResult.metaData));
            return Ok(pagedResult.employees);
        }
        /// <summary>
        /// Get empoyee for company by id company and id Employee
        /// </summary>
        /// <param name="companyId">Id company</param>
        /// <param name="id">Id employee</param>
        /// <returns>The employee objet dto</returns>
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

        /// <summary>
        /// Update an existing employee
        /// </summary>
        /// <param name="companyId">Id company</param>
        /// <param name="id">Id employee</param>
        /// <param name="employee">Employee object to update</param>
        /// <returns>No Content</returns>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateEmployeeForCompanyAsync(Guid companyId, Guid id,
                    [FromBody] EmployeeForUpdateDto employee)
        {
            await _sender.Send(new UpdateEmployeeForCompanyCommand(companyId, id, employee,
            compTrackChanges: false, empTrackChanges: true));
            return NoContent();
        }

        /// <summary>
        /// Delete existing company
        /// </summary>
        /// <param name="companyId">Id company</param>
        /// <param name="id">Id employee</param>
        /// <returns>No Content</returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEmployeeForCompanyAsync(Guid companyId, Guid id)
        {
            await _publisher.Publish(new EmployeeDeletedNotification(companyId, id, trackChanges:
            false));
            return NoContent();
        }
        
        /// <summary>
        /// Partially Update employee for company
        /// </summary>
        /// <param name="companyId">Id company</param>
        /// <param name="id">Id employee</param>
        /// <param name="patchDoc">Object contains properties to update or to remove</param>
        /// <returns>No Content</returns>
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
