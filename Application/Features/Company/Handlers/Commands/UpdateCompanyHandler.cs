using Application.Features.Company.Requests.Commands;
using AutoMapper;
using Contracts.Persistence;
using Entities.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Company.Handlers.Commands
{
    /*IRequestHandler always accepts two
parameters (TRequest and TResponse). So, we provide the Unit
structure for the TResponse parameter since it represents the void type.*/
    internal sealed class UpdateCompanyHandler : IRequestHandler<UpdateCompanyCommand, Unit>

    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public UpdateCompanyHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(UpdateCompanyCommand request, CancellationToken
        cancellationToken)
        {
            var companyEntity = await
                        _repository.Company.GetCompanyAsync(request.Id, request.TrackChanges);
            if (companyEntity is null)
                throw new CompanyNotFoundException(request.Id);
            _mapper.Map(request.Company, companyEntity);
            await _repository.SaveAsync();
            return Unit.Value;
        }
    }
}

