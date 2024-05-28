using MediatR;
using Shared.DataTransferObjects.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Company.Requests.Queries
{
    public sealed record GetCompanyQuery(Guid Id, bool TrackChanges) :
IRequest<CompanyDto>;
}
