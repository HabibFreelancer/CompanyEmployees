using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
   

    public record UserForRegistrationDto(string? FirstName, string? LastName, string? UserName, string? Password, string? Email , string? PhoneNumber,
        ICollection<string>? Roles);


}
