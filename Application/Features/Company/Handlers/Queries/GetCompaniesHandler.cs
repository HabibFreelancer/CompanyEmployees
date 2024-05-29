using Application.Features.Company.Requests.Queries;
using AutoMapper;
using Contracts.Persistence;
using Entities.Exceptions;
using MediatR;
using Shared.DataTransferObjects.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Company.Handlers.Queries
{
    /*Our handler inherits from IRequestHandler<GetCompaniesQuery,
IEnumerable<Product>>. This means this class will
handle GetCompaniesQuery, in this case, returning the list of companies.
    We also inject the repository through the constructor*/
    public sealed class GetCompaniesHandler : IRequestHandler<GetCompaniesQuery, IEnumerable<CompanyDto>>
                                                 , IRequestHandler<GetCompanyQuery, CompanyDto>
                                                    , IRequestHandler<GetCompaniesByIdsQuery, IEnumerable<CompanyDto>>
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
        public async Task<IEnumerable<CompanyDto>> Handle(GetCompaniesByIdsQuery request,
            CancellationToken cancellationToken)
        {


            if (request.ids is null)
                throw new IdParametersBadRequestException();
            var companyEntities = await _repository.Company.GetByIdsAsync(request.ids,
            request.TrackChanges);
            if (request.ids.Count() != companyEntities.Count())
                throw new CollectionByIdsBadRequestException();
            var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            return companiesToReturn;


        }
    }
}

