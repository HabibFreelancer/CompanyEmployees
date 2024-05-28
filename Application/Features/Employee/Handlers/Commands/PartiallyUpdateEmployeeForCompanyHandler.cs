using Application.Features.Employee.Requests.Commands;
using AutoMapper;
using Contracts.Persistence;
using Entities.Exceptions;
using Entities.Models;
using MediatR;
using Shared.DataTransferObjects.Employee;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Employee.Handlers.Commands
{
    internal sealed class PartiallyUpdateEmployeeForCompanyHandler : IRequestHandler<PartiallyUpdateEmployeeForCompanyCommand, Unit>
         , IRequestHandler<UpdateEmployeeForCompanyCommand, Unit>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public PartiallyUpdateEmployeeForCompanyHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(PartiallyUpdateEmployeeForCompanyCommand request, CancellationToken
        cancellationToken)
        {
            var result = await GetEmployeeForPatchAsync(request.companyId, request.id, request.compTrackChanges, request.empTrackChanges);
            request.patchDoc.ApplyTo(result.employeeToPatch);
            await SaveChangesForPatchAsync(result.employeeToPatch,
            result.employeeEntity);
            return Unit.Value;


        }

        public async Task<Unit> Handle(UpdateEmployeeForCompanyCommand request, CancellationToken cancellationToken)
        {
            await CheckIfCompanyExists(request.companyId, request.compTrackChanges);
            var employeeEntity = await GetEmployeeForCompanyAndCheckIfItExists(request.companyId, request.id, request.empTrackChanges);
            _mapper.Map(request.employeeForUpdate, employeeEntity);
            await _repository.SaveAsync();
            return Unit.Value;
        }

        private async Task<(EmployeeForUpdateDto employeeToPatch, Entities.Models.Employee employeeEntity)>
             GetEmployeeForPatchAsync
                 (Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges)
        {
            await CheckIfCompanyExists(companyId, compTrackChanges);


            var employeeEntity = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, empTrackChanges);
            var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);
            return (employeeToPatch, employeeEntity);
        }


        private async Task CheckIfCompanyExists(Guid companyId, bool trackChanges)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId,
            trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
        }
        private async Task<Entities.Models.Employee> GetEmployeeForCompanyAndCheckIfItExists
        (Guid companyId, Guid id, bool trackChanges)
        {
            var employeeDb = await _repository.Employee.GetEmployeeAsync(companyId, id,
            trackChanges);
            if (employeeDb is null)
                throw new EmployeeNotFoundException(id);
            return employeeDb;
        }

        private async Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Entities.Models.Employee
       employeeEntity)
        {
            _mapper.Map(employeeToPatch, employeeEntity);
            await _repository.SaveAsync();
        }


    }
}
