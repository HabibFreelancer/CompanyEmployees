using MediatR;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries
{
    public sealed record GetEmployeesForCompanyQuery(Guid companyId,
             EmployeeParameters employeeParameters, bool trackChanges) :
             IRequest<(IEnumerable<ExpandoObject> employees, MetaData metaData)>;
}
