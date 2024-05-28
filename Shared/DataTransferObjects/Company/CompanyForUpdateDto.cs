using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DataTransferObjects.Employee;

namespace Shared.DataTransferObjects.Company
{
    public record CompanyForUpdateDto(string Name, string Address, string Country,
                IEnumerable<EmployeeForCreationDto> Employees);
}
