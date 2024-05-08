using MediatR;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries
{
    public sealed record GetCompaniesByIdsQuery(IEnumerable<Guid> ids,bool TrackChanges) :
IRequest<IEnumerable<CompanyDto>>;
}
