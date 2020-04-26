using CodeGenerator.BusinessService.IService;
using CodeGenerator.BusinessService.Common;
using CodeGenerator.Util;
using Microsoft.AspNetCore.Mvc;

namespace CodeGenerator.Web.Areas.Admin.Controllers
{

    public class HomeController : BaseMvcController
    {
        private IHomeService _homeBus { get; }
        public HomeController(IHomeService homebus)
        {
            _homeBus = homebus;
        }


        #region 视图功能

        public ActionResult Index()
        {
            return View();
        }

        [IgnoreLogin]
        public ActionResult Login()
        {
            if (Operator.Logged())
            {

                string loginUrl = Url.Content("~/");
                string script = $@"    
<html>
    <script>
        top.location.href = '{loginUrl}';
    </script>
</html>
";
                return Content(script, "text/html");
            }

            return View();
        }

        public ActionResult Desktop()
        {
            return View();
        }

        #endregion

        #region 获取数据

        #endregion

        #region 提交数据

        [IgnoreLogin]
        public ActionResult SubmitLogin(string userName, string password)
        {
            AjaxResult res = _homeBus.SubmitLogin(userName, password);

            return Content(res.ToJson());
        }

        /// <summary>
        /// 注销
        /// </summary>
        public ActionResult Logout()
        {
            Operator.Logout();

            return Success();
        }

        #endregion
    }
}