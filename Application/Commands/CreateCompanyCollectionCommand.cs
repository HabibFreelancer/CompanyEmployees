using MediatR;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
   
    /* public sealed record CreateCompanyCommand(CompanyForCreationDto Company) :
 IRequest<CompanyDto>;*/
    /*Task<(IEnumerable<CompanyDto> companies, string ids)>
        CreateCompanyCollectionAsync
        (IEnumerable<CompanyForCreationDto> companyCollection);*/
    public sealed record CreateCompanyCollectionCommand(IEnumerable<CompanyForCreationDto> companies)
        : IRequest<(IEnumerable<CompanyDto> companies, string ids)>;



}
