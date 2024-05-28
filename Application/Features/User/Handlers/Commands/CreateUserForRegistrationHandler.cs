using Application.Features.User.Requests.Commands;
using AutoMapper;
using Contracts;
using Entities.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.User.Handlers.Commands
{
    internal sealed class CreateUserForRegistrationHandler : IRequestHandler<CreateUserForRegistrationCommand, IdentityResult>
    {


        private readonly IMapper _mapper;
        private readonly UserManager<Entities.Models.User> _userManager;
        public CreateUserForRegistrationHandler(IMapper mapper, UserManager<Entities.Models.User> userManager)
        {

            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IdentityResult> Handle(CreateUserForRegistrationCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<Entities.Models.User>(request.userForRegistration);
            var result = await _userManager.CreateAsync(user,
           request.userForRegistration.Password);
            if (result.Succeeded)
                await _userManager.AddToRolesAsync(user, request.userForRegistration.Roles);
            return result;
        }
    }
}
