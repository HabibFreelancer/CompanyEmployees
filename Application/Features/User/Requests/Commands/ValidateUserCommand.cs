using MediatR;
using Shared.DataTransferObjects.Auth;
using Shared.DataTransferObjects.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.User.Requests.Commands
{


    public sealed record ValidateUserCommand(bool populateExp, UserForAuthenticationDto userForAuth) : IRequest<(bool, TokenDto?)>;

}
