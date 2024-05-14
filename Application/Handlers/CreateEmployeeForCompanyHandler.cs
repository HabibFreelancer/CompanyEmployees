using Application.Commands;
using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using MediatR;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    internal sealed class CreateEmployeeForCompanyHandler : IRequestHandler<CreateEmployeeForCompanyCommand,
   EmployeeDto>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public CreateEmployeeForCompanyHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<EmployeeDto> Handle(CreateEmployeeForCompanyCommand request,
        CancellationToken cancellationToken)
        {
            await CheckIfCompanyExists(request.companyId, request.trackChanges);
            var employeeEntity = _mapper.Map<Employee>(request.employeeForCreation);
            _repository.Employee.CreateEmployeeForCompany(request.companyId, employeeEntity);
            await _repository.SaveAsync();
            var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);
            return employeeToReturn;
        }

        private async Task CheckIfCompanyExists(Guid companyId, bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId,
            trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
        }

    }
}
