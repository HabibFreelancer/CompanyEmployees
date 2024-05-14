using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{

    public sealed record PartiallyUpdateEmployeeForCompanyCommand(JsonPatchDocument<EmployeeForUpdateDto> patchDoc ,Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges) 
        : IRequest<Unit>;
   
}
