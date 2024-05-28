using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.DataTransferObjects.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.User.Requests.Commands
{
    public sealed record CreateUserForRegistrationCommand(UserForRegistrationDto userForRegistration) : IRequest<IdentityResult>;

}
