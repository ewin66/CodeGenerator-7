using System.Collections.Generic;
using CodeGenerator.BusinessService.Base_SysManage;
using CodeGenerator.Entity.Base_SysManage;
using CodeGenerator.Util;

namespace CodeGenerator.BusinessService.IService
{
    public interface IBase_UserService
    {

        #region 外部接口

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="condition">查询类型</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        List<Base_UserModel> GetDataList(string condition, string keyword, Pagination pagination);

        /// <summary>
        /// 获取指定的单条数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Base_User GetTheData(string id);





        void AddData(Base_User newData);

        /// <summary>
        /// 更新数据
        /// </summary>
        void UpdateData(Base_User theData);

        void SetUserRole(string userId, List<string> roleIds);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="theData">删除的数据</param>
        void DeleteData(List<string> ids);


        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="oldPwd">老密码</param>
        /// <param name="newPwd">新密码</param>
        AjaxResult ChangePwd(string oldPwd, string newPwd);


        /// <summary>
        /// 保存权限
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="permissions">权限值</param>
        void SavePermission(string userId, List<string> permissions);
       

        #endregion

    }
}