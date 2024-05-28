using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Employee.Notifications.Commands
{
    public sealed record EmployeeDeletedNotification(Guid companyId, Guid id, bool trackChanges) :
INotification;
}
