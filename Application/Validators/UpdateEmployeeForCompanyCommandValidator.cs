using Application.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public sealed class UpdateEmployeeForCompanyCommandValidator :
AbstractValidator<UpdateEmployeeForCompanyCommand>
    {

        public UpdateEmployeeForCompanyCommandValidator()
        {
            RuleFor(e => e.employeeForUpdate.Name).NotEmpty().MaximumLength(60);
            RuleFor(e => e.employeeForUpdate.Age).NotEmpty().InclusiveBetween(18, int.MaxValue);
            RuleFor(e => e.employeeForUpdate.Position).NotEmpty().MaximumLength(20);
        }
    }
}
