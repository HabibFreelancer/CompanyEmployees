using Application.Commands;
using Contracts;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    
    public sealed class CreateCompanyCommandValidator :
AbstractValidator<CreateCompanyCommand>
    {
        private readonly IRepositoryManager _repository;
        public CreateCompanyCommandValidator(IRepositoryManager repository)
        {
            _repository = repository;
            RuleFor(c => c.Company.Name)
                .NotEmpty().MaximumLength(60)
                .MustAsync(LeaveTypeNameUnique);
            RuleFor(c => c.Company.Address).NotEmpty().MaximumLength(60);
            RuleFor(c => c.Company.Address).NotEmpty().MaximumLength(60);
        }
        private Task<bool> LeaveTypeNameUnique(string name, CancellationToken token)
        {
            return _repository.Company.IsCompanyUnique(name);
        }

        /* Validating null Object :Our recommendation is
to use 422 only for the validation errors, and 400 if the request body is
null.*/
        //        public override ValidationResult
        //Validate(ValidationContext<CreateCompanyCommand> context)
        //        {
        //            return context.InstanceToValidate.Company is null
        //            ? new ValidationResult(new[] { new
        //ValidationFailure("CompanyForCreationDto",
        //"CompanyForCreationDto object is null") })
        //            : base.Validate(context);
        //        }
    }
}
