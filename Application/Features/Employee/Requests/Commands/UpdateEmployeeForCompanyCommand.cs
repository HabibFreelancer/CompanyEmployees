using MediatR;
using Shared.DataTransferObjects.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Employee.Requests.Commands
{
    public sealed record UpdateEmployeeForCompanyCommand(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges) : IRequest<Unit>;

}
