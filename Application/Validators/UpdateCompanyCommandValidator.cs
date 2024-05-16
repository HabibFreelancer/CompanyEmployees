using Application.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public sealed class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
    {
        public UpdateCompanyCommandValidator()
        {
            RuleFor(c => c.Company.Name).NotEmpty().MaximumLength(60);
            RuleFor(c => c.Company.Address).NotEmpty().MaximumLength(60);
        }
    }
}
