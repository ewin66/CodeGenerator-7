using CodeGenerator.Util;

namespace CodeGenerator.BusinessService.IService
{
    public interface IHomeService
    {
        AjaxResult SubmitLogin(string userName, string password);
    }
}