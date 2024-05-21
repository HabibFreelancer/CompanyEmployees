using Application.Commands;
using Application.Queries;
using CompanyEmployees.Presentation.ActionFilters;
using Entities.Models;
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
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ISender _sender;
        private User? _user;
        public AuthenticationController(ISender sender) => _sender = sender;

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            var result = await
             _sender.Send(new CreateUserForRegistrationCommand(userForRegistration));
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
        {

            var result = await _sender.Send(new ValidateUserCommand(populateExp: true,user)) ;
            if (!result.Item1)
                return Unauthorized();
            return Ok(result.Item2);
        }

    }
}
