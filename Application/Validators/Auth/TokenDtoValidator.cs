using FluentValidation;
using Shared.DataTransferObjects.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Auth
{
    public sealed class TokenDtoValidator : AbstractValidator<TokenDto>
    {
        public TokenDtoValidator()
        {
            RuleFor(t => t.AccessToken).NotEmpty();
            RuleFor(t => t.RefreshToken).NotEmpty();


        }
    }
}
