using Microsoft.AspNetCore.Http;

namespace CodeGenerator.Util
{
    public static class HttpContextCore
    {
        public static HttpContext Current { get => AutofacHelper.GetService<IHttpContextAccessor>().HttpContext; }
    }
}
