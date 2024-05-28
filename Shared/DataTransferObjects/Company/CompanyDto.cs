using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects.Company
{
    public record CompanyDto
    {
        public Guid Id { get; init; } /*init to enable the XML serializer of field*/
        public string? Name { get; init; }
        public string? FullAddress { get; init; }
    }
}
