using MediatR;
using Shared.DataTransferObjects.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Company.Requests.Commands
{


    public sealed record CreateCompanyCollectionCommand(IEnumerable<CompanyForCreationDto> companies)
        : IRequest<(IEnumerable<CompanyDto> companies, string ids)>;



}
