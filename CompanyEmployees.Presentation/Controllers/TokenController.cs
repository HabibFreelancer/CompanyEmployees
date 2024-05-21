using Application.Commands;
using Application.Queries;
using CompanyEmployees.Presentation.ActionFilters;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ISender _sender;
        public TokenController(ISender sender) => _sender = sender;

        [HttpPost("refresh")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
        {
            var tokenDtoToReturn = await _sender.Send(new RefreshTokenCommand(tokenDto));
            return Ok(tokenDtoToReturn);
        }
    }
}
