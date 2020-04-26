using CodeGenerator.BusinessService.Common;
using CodeGenerator.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CodeGenerator.WebApi
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;
            BusHelper.HandleException(ex);

            context.Result = new ContentResult { Content = new AjaxResult { Success = false, Msg = ex.Message }.ToJson() };
        }
    }
}
