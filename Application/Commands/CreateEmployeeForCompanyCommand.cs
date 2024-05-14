using MediatR;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
    public sealed record CreateEmployeeForCompanyCommand(Guid companyId, EmployeeForCreationDto
                                                        employeeForCreation, bool trackChanges) :
 IRequest<EmployeeDto>;
}
