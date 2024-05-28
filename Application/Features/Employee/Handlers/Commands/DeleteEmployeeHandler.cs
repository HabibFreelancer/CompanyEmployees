using Application.Features.Employee.Notifications.Commands;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Application.Features.Employee.Handlers.Commands
{
    internal sealed class DeleteEmployeeHandler : INotificationHandler<EmployeeDeletedNotification>
    {
        private readonly IRepositoryManager _repository;
        public DeleteEmployeeHandler(IRepositoryManager repository) => _repository = repository;
        public async Task Handle(EmployeeDeletedNotification notification,
 CancellationToken cancellationToken)
        {
            var employee = await _repository.Employee.GetEmployeeAsync(notification.companyId, notification.id, notification.trackChanges);

            if (employee is null)
                throw new EmployeeNotFoundException(notification.id);
            _repository.Employee.DeleteEmployee(employee);
            await _repository.SaveAsync();


        }

    }
}
