using Application.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
  
    public sealed class ValidateUserCommandValidator :
AbstractValidator<ValidateUserCommand>
    {
        public ValidateUserCommandValidator()
        {
            RuleFor(u => u.userForAuth.UserName).NotEmpty();
            RuleFor(u => u.userForAuth.Password).NotEmpty();


        }
    }
}
