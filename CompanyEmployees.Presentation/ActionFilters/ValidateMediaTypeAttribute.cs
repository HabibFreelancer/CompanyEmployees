using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace CompanyEmployees.Presentation.ActionFilters
{
    /*implement an ActionFilter in the Presentation project
inside the ActionFilters folder, which will validate our Accept header
and media types*/
    public class ValidateMediaTypeAttribute : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var acceptHeaderPresent = context.HttpContext
            .Request.Headers.ContainsKey("Accept");
            if (!acceptHeaderPresent)
            {
                context.Result = new BadRequestObjectResult($"Accept header i smissing.");
              return;
            }
            var mediaType = context.HttpContext.Request.Headers["Accept"].FirstOrDefault();
            /*we parse the media type — and if there is
no valid media type present, we return BadRequest*/
            if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue? outMediaType))
            {
                context.Result = new BadRequestObjectResult($"Media type not present.Please add Accept header with the required media type.");
            return;
            }
            context.HttpContext.Items.Add("AcceptHeaderMediaType", outMediaType);
        }
        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
