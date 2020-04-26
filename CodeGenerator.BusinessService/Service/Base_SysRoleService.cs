using CodeGenerator.BusinessService.Cache;
using CodeGenerator.Entity.Base_SysManage;
using CodeGenerator.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using CodeGenerator.BusinessService.IService;

namespace CodeGenerator.BusinessService.Base_SysManage
{
    public class Base_SysRoleService : BaseService<Base_SysRole>, IBase_SysRoleService
    {
        static Base_SysRoleCache _cache { get; } = new Base_SysRoleCache();
        #region 外部接口

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="condition">查询类型</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public List<Base_SysRole> GetDataList(string condition, string keyword, Pagination pagination)
        {
            var q = GetIQueryable();

            //模糊查询
            if (!condition.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
                q = q.Where($@"{condition}.Contains(@0)", keyword);

            return q.GetPagination(pagination).ToList();
        }

        /// <summary>
        /// 获取指定的单条数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public Base_SysRole GetTheData(string id)
        {
            return GetEntity(id);
        }

        public static string GetRoleName(string userId)
        {
            return _cache.GetCache(userId)?.RoleName;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="newData">数据</param>
        public void AddData(Base_SysRole newData)
        {
            Insert(newData);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        public void UpdateData(Base_SysRole theData)
        {
            Modify(theData);
            _cache.UpdateCache(theData.RoleId);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="theData">删除的数据</param>
        public void DeleteData(List<string> ids)
        {
            var roleIds = GetIQueryable().Where(x => ids.Contains(x.RoleId)).Select(x => x.RoleId).ToList();
            //删除角色
            Delete(ids);
            _cache.UpdateCache(roleIds);
        }

        /// <summary>
        /// 保存权限
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="permissions">权限值</param>
        public void SavePermission(string roleId,List<string> permissions)
        {
            Service.Delete<Base_PermissionRole>(x => x.RoleId == roleId);
            List<Base_PermissionRole> insertList = new List<Base_PermissionRole>();
            permissions.ForEach(newPermission =>
            {
                insertList.Add(new Base_PermissionRole
                {
                    Id=Guid.NewGuid().ToSequentialGuid(),
                    RoleId=roleId,
                    PermissionValue=newPermission
                });
            });

            Service.Insert(insertList);
        }

        #endregion

        #region 私有成员

        #endregion

        #region 数据模型

        #endregion
    }
}