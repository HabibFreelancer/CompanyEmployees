using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.ActionFilters
{
    public class ValidationFilterAttribute : IActionFilter
    {
        public ValidationFilterAttribute()
        { }
        public void OnActionExecuting(ActionExecutingContext context)
        {

            /*With the RouteData.Values dictionary, we can
get the values produced by routes on the current routing path. Since we
need the name of the action and the controller, we extract them from the
Values dictionary.*/
            var action = context.RouteData.Values["action"];
            var controller = context.RouteData.Values["controller"];

            /*we use the ActionArguments dictionary to extract the DTO
parameter that we send to the POST and PUT actions.*/
            var param = context.ActionArguments
                .SingleOrDefault(x => x.Value.ToString().Contains("Dto")).Value;
            if (param is null)
            {
                /*we set the Result property of the context object to a new instance
of the BadRequestObjectReturnResult class*/
                context.Result = new BadRequestObjectResult($"Object is null. Controller:{{ controller }}, action: {{ action}} ");
                return;
            }
            if (!context.ModelState.IsValid)
                /*we create a new instance of the UnprocessableEntityObjectResult
class and pass ModelState*/
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);

        }
        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
