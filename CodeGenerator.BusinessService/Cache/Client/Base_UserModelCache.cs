using CodeGenerator.BusinessService.Base_SysManage;
using CodeGenerator.Util;
using System.Linq;

namespace CodeGenerator.BusinessService.Cache
{
    public class Base_UserModelCache : BaseCache<Base_UserModel>
    {
        public Base_UserModelCache()
            : base("Base_UserModel", userId =>
            {
                if (userId.IsNullOrEmpty())
                    return null;
                return new Base_UserService().GetDataList("UserId", userId, new Pagination()).FirstOrDefault();
            })
        {

        }
    }
}
