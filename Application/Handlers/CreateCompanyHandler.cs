using Application.Commands;
using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using MediatR;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    internal sealed class CreateCompanyHandler : IRequestHandler<CreateCompanyCommand, CompanyDto>
                                                  , IRequestHandler<CreateCompanyCollectionCommand, (IEnumerable<CompanyDto> companies, string ids)>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public CreateCompanyHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<CompanyDto> Handle(CreateCompanyCommand request,
        CancellationToken cancellationToken)
        {
            var companyEntity = _mapper.Map<Company>(request.Company);
            _repository.Company.CreateCompany(companyEntity);
            await _repository.SaveAsync();
            var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);
            return companyToReturn;
        }
        public async Task<(IEnumerable<CompanyDto> companies, string ids)> Handle(CreateCompanyCollectionCommand request,
             CancellationToken cancellationToken)
        {
            if (request.companies is null)
                throw new CompanyCollectionBadRequest();
            var companyEntities = _mapper.Map<IEnumerable<Company>>(request.companies);
            foreach (var company in companyEntities)
            {
                _repository.Company.CreateCompany(company);
            }
            await _repository.SaveAsync();
            var companyCollectionToReturn =
         _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));
            return (companies: companyCollectionToReturn, ids: ids);

        }
    }
}
