using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NoriaBE.Filters;

public class ExceptionFilter: ActionFilterAttribute
{

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception != null)
        {
            var result = new
            {
                success = false,
                message = context.Exception.Message
            };
            context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(result)
            {
                StatusCode = 500,
            };
            context.ExceptionHandled = true;
        }
        base.OnActionExecuted(context);
    }
}
