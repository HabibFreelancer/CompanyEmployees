using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
    public sealed record CreateUserForRegistrationCommand(UserForRegistrationDto userForRegistration) : IRequest<IdentityResult>;
    
}
