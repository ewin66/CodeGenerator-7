using CodeGenerator.BusinessService.Common;
using CodeGenerator.BusinessService.IService;
using CodeGenerator.Entity.Base_SysManage;
using CodeGenerator.Util;
using System.Linq;

namespace CodeGenerator.BusinessService.Base_SysManage
{
    public class HomeService : BaseService<Base_User>, IHomeService
    {
        public AjaxResult SubmitLogin(string userName, string password)
        {
            if (userName.IsNullOrEmpty() || password.IsNullOrEmpty())
                return Error("账号或密码不能为空！");
            password = password.ToMD5String();
            var theUser = GetIQueryable().Where(x => x.UserName == userName && x.Password == password).FirstOrDefault();
            if (theUser != null)
            {
                Operator.Login(theUser.UserId);
                return Success();
            }
            else
                return Error("账号或密码不正确！");
        }
    }
}