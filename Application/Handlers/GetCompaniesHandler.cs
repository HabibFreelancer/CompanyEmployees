using Application.Queries;
using AutoMapper;
using Contracts;
using Entities.Exceptions;
using MediatR;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    /*Our handler inherits from IRequestHandler<GetCompaniesQuery,
IEnumerable<Product>>. This means this class will
handle GetCompaniesQuery, in this case, returning the list of companies.
    We also inject the repository through the constructor*/
    internal sealed class GetCompaniesHandler : IRequestHandler<GetCompaniesQuery,
IEnumerable<CompanyDto>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public GetCompaniesHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CompanyDto>> Handle(GetCompaniesQuery request,
        CancellationToken cancellationToken)
        {
            var companies = await _repository.Company.GetAllCompaniesAsync(request.TrackChanges);

            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return companiesDto;
        }

        public async Task<CompanyDto> Handle(GetCompanyQuery request, CancellationToken
cancellationToken)
        {
            var company = await _repository.Company.GetCompanyAsync(request.Id,
            request.TrackChanges);
            if (company is null)
                throw new CompanyNotFoundException(request.Id);
            var companyDto = _mapper.Map<CompanyDto>(company);
            return companyDto;

        }
    }
}

