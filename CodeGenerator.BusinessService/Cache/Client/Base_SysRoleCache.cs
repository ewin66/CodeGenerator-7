using CodeGenerator.DataRepository;
using CodeGenerator.Entity.Base_SysManage;
using System.Linq;

namespace CodeGenerator.BusinessService.Cache
{
    class Base_SysRoleCache : BaseCache<Base_SysRole>
    {
        public Base_SysRoleCache()
            : base("UserRoleCache", roleId =>
            {
                return DbFactory.GetRepository().GetIQueryable<Base_SysRole>().Where(x => x.RoleId == roleId).FirstOrDefault();
            })
        {

        }
    }
}
