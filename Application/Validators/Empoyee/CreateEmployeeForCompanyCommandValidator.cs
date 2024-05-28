using Application.Features.Employee.Requests.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.Empoyee
{
    public sealed class CreateEmployeeForCompanyCommandValidator :
AbstractValidator<CreateEmployeeForCompanyCommand>
    {
        public CreateEmployeeForCompanyCommandValidator()
        {
            RuleFor(e => e.employeeForCreation.Name).NotEmpty().MaximumLength(60);
            RuleFor(e => e.employeeForCreation.Age).NotEmpty().InclusiveBetween(18, int.MaxValue);
            RuleFor(e => e.employeeForCreation.Position).NotEmpty().MaximumLength(20);
        }
    }
}
