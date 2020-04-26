using CodeGenerator.DataRepository;
using CodeGenerator.Entity.Crm;
using System.Linq;

namespace CodeGenerator.BusinessService.Cache
{
    class Api_UserToken : BaseCache<Crm_Customer>
    {
        public Api_UserToken()
            : base("UserRoleCache", userId =>
            {
                return DbFactory.GetRepository().GetIQueryable<Crm_Customer>().Where(x => x.CustomerId == userId).FirstOrDefault();
            })
        {

        }
    }
}
