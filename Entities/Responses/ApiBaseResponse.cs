using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Responses
{
    /*This is an abstract class, which will be the main return type for all of our
methods where we have to return a successful result or an error result. It
also contains a single Success property stating whether the action was
successful or not*/
    public class ApiBaseResponse
    {
        public bool Success { get; set; }
        protected ApiBaseResponse(bool success) => Success = success;
    }
}
