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
        /*Example calling URL : https://localhost:7050/api/companies/c9d4c053-49b6-410c-bc78-2d54a9991870/employees?pageNumber=2&pageSize=2*/
        /// <summary>
        /// Gets the list of all employee by company
        /// </summary>
        /// <param name="companyId">Id company</param>
        /// <param name="employeeParameters"> Filter parameters (max Age, min Age , search term ....)</param>
        /// <returns>The employee list with paging</returns>
        [HttpGet]
        [HttpHead] /*Head HTTP Request :The Head is identical to Get but without a response body. This type of request could be used to obtain information about validity, accessibility, and recent modifications of the resource.
the Head request must return the same response as the Get request — just without the response body.
*/
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetEmployeesForCompanyAsync(Guid companyId,
            [FromQuery] EmployeeParameters employeeParameters)
        {
            var pagedResult = await _sender.Send(new GetEmployeesForCompanyQuery(companyId, employeeParameters, trackChanges: false));
            Response.Headers.Add("X-Pagination",
                        JsonSerializer.Serialize(pagedResult.metaData));
            return Ok(pagedResult.employees);
        }
        /*Example calling URL : https://localhost:7050/api/companies/c9d4c053-49b6-410c-bc78-2d54a9991870/employees/6df55ff3-6437-4f30-cc89-08dc605ba8b0*/
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
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
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
        /*Example calling URL : https://localhost:7050/api/companies/c9d4c053-49b6-410c-bc78-2d54a9991870/employees/4f18abe8-3bff-4fd9-110f-08dc767b0fde*/
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
        /*Example calling URL : https://localhost:7050/api/companies/c9d4c053-49b6-410c-bc78-2d54a9991870/employees/4f18abe8-3bff-4fd9-110f-08dc767b0fde*/
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
        /*Example Calling URL : https://localhost:7050/api/companies/c9d4c053-49b6-410c-bc78-2d54a9991870/employees/f697b63b-70db-4f25-110e-08dc767b0fde*/
        /*We Should Pass Content-Type : application/json-patch+json in Request Header */
        /* Example of passing object :
         * [
             {
                 "op": "replace",
                 "path": "/name",
                 "value": "new name replace "
             }
            ]
         */
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
