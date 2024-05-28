using MediatR;
using Shared.DataTransferObjects.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Employee.Requests.Queries
{
    public sealed record GetEmployeeForCompanyByIdQuery(Guid companyId, Guid id, bool trackChanges) :
             IRequest<EmployeeDto>;
}
