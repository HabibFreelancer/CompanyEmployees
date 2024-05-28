using Application.Features.Employee.Requests.Queries;
using AutoMapper;
using Contracts.Infrastructure;
using Contracts.Persistence;
using Entities.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.DataTransferObjects.Employee;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Employee.Handlers.Queries
{
    internal sealed class GetEmployeesForCompanyHandler :
        IRequestHandler<GetEmployeesForCompanyQuery, (IEnumerable<ExpandoObject> employees, MetaData metaData)>,
        IRequestHandler<GetEmployeeForCompanyByIdQuery, EmployeeDto>
    {


        private readonly ILoggerManager<GetEmployeesForCompanyHandler> _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IDataShaper<EmployeeDto> _dataShaper;
        public GetEmployeesForCompanyHandler(IRepositoryManager repository, IMapper mapper, ILoggerManager<GetEmployeesForCompanyHandler>
        logger, IDataShaper<EmployeeDto> dataShaper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _dataShaper = dataShaper;
        }

        public async Task<(IEnumerable<ExpandoObject> employees, MetaData metaData)> Handle(GetEmployeesForCompanyQuery request,
        CancellationToken cancellationToken)
        {
            if (!request.employeeParameters.ValidAgeRange)
                throw new MaxAgeRangeBadRequestException();

            await CheckIfCompanyExists(request.companyId, request.trackChanges);
            var employeesWithMetaData = await _repository.Employee.GetEmployeesAsync(request.companyId,
                                                            request.employeeParameters, request.trackChanges);
            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetaData);
            var shapedData = _dataShaper.ShapeData(employeesDto, request.employeeParameters.Fields);
            return (employees: shapedData, metaData: employeesWithMetaData.MetaData);

        }


        public async Task<EmployeeDto> Handle(GetEmployeeForCompanyByIdQuery request,
       CancellationToken cancellationToken)
        {
            await CheckIfCompanyExists(request.companyId, request.trackChanges);
            var employeeDb = await _repository.Employee.GetEmployeeAsync(request.companyId, request.id, request.trackChanges);
            if (employeeDb is null)
                throw new EmployeeNotFoundException(request.id);
            var employee = _mapper.Map<EmployeeDto>(employeeDb);
            return employee;
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
