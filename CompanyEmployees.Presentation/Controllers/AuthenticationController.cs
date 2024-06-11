using CompanyEmployees.Presentation.ActionFilters;
using Contracts.Identity;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObjects;
using Shared.DataTransferObjects.Auth;
using Shared.DataTransferObjects.User;
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
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService) => _authenticationService = authenticationService;

        [HttpPost("register")]
        public async Task<ActionResult<RegistrationResponse>> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            var result = await
            _authenticationService.RegisterUser(userForRegistration);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            return StatusCode(201, new RegistrationResponse() { Email = userForRegistration.Email });
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] UserForAuthenticationDto user)
        {
            if (!await _authenticationService.ValidateUser(user))
                return Unauthorized();
            var response = await _authenticationService.CreateToken(populateExp: true);
            return Ok(response);
        }

    }
}