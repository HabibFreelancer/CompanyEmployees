using Application.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public sealed class RefreshTokenCommandValidator :
AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(t => t.tokenDto.AccessToken).NotEmpty();
            RuleFor(t => t.tokenDto.RefreshToken).NotEmpty();


        }
    }
}
