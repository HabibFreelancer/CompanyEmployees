using Application.Features.Company.Notifications.Requests;
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
    internal sealed class DeleteCompanyHandler : INotificationHandler<CompanyDeletedNotification>
    {
        private readonly IRepositoryManager _repository;
        public DeleteCompanyHandler(IRepositoryManager repository) => _repository =
        repository;
        public async Task Handle(CompanyDeletedNotification notification,
 CancellationToken cancellationToken)
        {
            var company = await _repository.Company.GetCompanyAsync(notification.Id,
                                                                        notification.TrackChanges);
            if (company is null)
                throw new CompanyNotFoundException(notification.Id);
            _repository.Company.DeleteCompany(company);
            await _repository.SaveAsync();
        }
    }
}
