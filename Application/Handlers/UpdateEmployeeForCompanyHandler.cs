using Application.Commands;
using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{

    internal sealed class UpdateEmployeeForCompanyHandler : IRequestHandler<UpdateEmployeeForCompanyCommand, Unit>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public UpdateEmployeeForCompanyHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateEmployeeForCompanyCommand request, CancellationToken
        cancellationToken)
        {

            await CheckIfCompanyExists(request.companyId, request.compTrackChanges);
            var employeeEntity = await GetEmployeeForCompanyAndCheckIfItExists(request.companyId, request.id, request.empTrackChanges);
            await _repository.SaveAsync();
            return Unit.Value;

        }

        private async Task CheckIfCompanyExists(Guid companyId, bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId,
            trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
        }
        private async Task<Employee> GetEmployeeForCompanyAndCheckIfItExists
        (Guid companyId, Guid id, bool trackChanges)
        {
            var employeeDb = await _repository.Employee.GetEmployeeAsync(companyId, id,
            trackChanges);
            if (employeeDb is null)
                throw new EmployeeNotFoundException(id);
            return employeeDb;
        }

    }
}
