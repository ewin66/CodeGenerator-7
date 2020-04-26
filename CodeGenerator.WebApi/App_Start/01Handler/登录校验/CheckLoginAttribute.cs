using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeGenerator.BusinessService.Common;
using CodeGenerator.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace CodeGenerator.WebApi
{
    public class CheckLoginAttribute : Attribute, IActionFilter
    {
        /// <summary>
        /// Action执行完毕之前执行
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            try
            {
                //判断是否需要登录
                List<string> attrList = FilterHelper.GetFilterList(filterContext);
                bool needLogin = attrList.Contains(typeof(CheckLoginAttribute).FullName) && !attrList.Contains(typeof(IgnoreLoginAttribute).FullName);

                //转到登录
                if (needLogin && !Operator.Logged())
                {
                    RedirectToLogin();
                }
            }
            catch (Exception ex)
            {
                BusHelper.HandleException(ex);
                RedirectToLogin();
            }

            void RedirectToLogin()
            {
                filterContext.Result = new ContentResult
                {
                    Content = new AjaxResult { Success = false, Code = 401, Msg = "未登录" }.ToJson(),
                    ContentType = "application/json;charset=UTF-8"
                };
            }
        }

        /// <summary>
        /// Action执行完毕之后执行
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}
