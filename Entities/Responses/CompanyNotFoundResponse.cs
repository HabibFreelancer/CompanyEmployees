using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Responses
{

    /*The class inherits from the ApiNotFoundResponse abstract class, which
again inherits from the ApiBaseResponse class. It accepts an id
parameter and creates a message that sends to the base class*/
    public sealed class CompanyNotFoundResponse : ApiNotFoundResponse
    {
        public CompanyNotFoundResponse(Guid id)
        : base($"Company with id: {id} is not found in db.")
        { }
    }
}
