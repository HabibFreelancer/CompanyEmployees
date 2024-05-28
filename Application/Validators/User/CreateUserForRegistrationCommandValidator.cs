using Application.Features.User.Requests.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.User
{

    public sealed class CreateUserForRegistrationCommandValidator :
        AbstractValidator<CreateUserForRegistrationCommand>
    {
        public CreateUserForRegistrationCommandValidator()
        {
            RuleFor(u => u.userForRegistration.UserName).NotEmpty();
            RuleFor(u => u.userForRegistration.Password).NotEmpty();

        }


    }
}
