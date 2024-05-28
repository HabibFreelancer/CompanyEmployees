using Application.Features.Company.Requests.Commands;
using AutoMapper;
using Contracts;
using Entities.Exceptions;
using MediatR;
using Shared.DataTransferObjects.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Company.Handlers.Commands
{
    internal sealed class CreateCompanyCollectionHandler : IRequestHandler<CreateCompanyCollectionCommand, (IEnumerable<CompanyDto> companies, string ids)>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public CreateCompanyCollectionHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<(IEnumerable<CompanyDto> companies, string ids)> Handle(CreateCompanyCollectionCommand request,
            CancellationToken cancellationToken)
        {
            if (request.companies is null)
                throw new CompanyCollectionBadRequest();
            var companyEntities = _mapper.Map<IEnumerable<Entities.Models.Company>>(request.companies);
            foreach (var company in companyEntities)
            {
                _repository.Company.CreateCompany(company);
            }
            await _repository.SaveAsync();
            var companyCollectionToReturn =
         _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));
            return (companies: companyCollectionToReturn, ids);

        }
    }
}
