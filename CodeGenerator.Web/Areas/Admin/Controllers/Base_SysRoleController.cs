using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CodeGenerator.BusinessService.IService;
using CodeGenerator.BusinessService.Tool;
using CodeGenerator.Entity.Base_SysManage;
using CodeGenerator.Util;

namespace CodeGenerator.Web.Areas.Admin.Controllers
{
    public class Base_SysRoleController : BaseMvcController
    {
        private IBase_SysRoleService _base_SysRoleService { get; }
        public Base_SysRoleController(IBase_SysRoleService base_SysRoleService) {
            _base_SysRoleService = base_SysRoleService;
        }

        #region 视图功能

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form(string id)
        {
            var theData = id.IsNullOrEmpty() ? new Base_SysRole() : _base_SysRoleService.GetTheData(id);

            return View(theData);
        }

        public ActionResult PermissionForm(string roleId)
        {
            ViewData["roleId"] = roleId;

            return View();
        }

        #endregion

        #region 获取数据

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="condition">查询类型</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public ActionResult GetDataList(string condition, string keyword, Pagination pagination)
        {
            var dataList = _base_SysRoleService.GetDataList(condition, keyword, pagination);

            return Content(pagination.BuildTableResult_DataGrid(dataList).ToJson());
        }

        /// <summary>
        /// 获取角色列表
        /// 注：无分页
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDataList_NoPagin()
        {
            Pagination pagination = new Pagination
            {
                PageIndex = 1,
                PageRows = int.MaxValue
            };
            var dataList = _base_SysRoleService.GetDataList(null, null, pagination);

            return Content(dataList.ToJson());
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="theData">保存的数据</param>
        public ActionResult SaveData(Base_SysRole theData)
        {
            if (theData.Id.IsNullOrEmpty())
            {
                theData.Id = Guid.NewGuid().ToSequentialGuid();
                theData.RoleId = Guid.NewGuid().ToSequentialGuid();

                _base_SysRoleService.AddData(theData);
            }
            else
            {
                _base_SysRoleService.UpdateData(theData);
            }

            return Success();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="theData">删除的数据</param>
        public ActionResult DeleteData(string ids)
        {
            _base_SysRoleService.DeleteData(ids.ToList<string>());

            PermissionManage.ClearUserPermissionCache();

            return Success("删除成功！");
        }

        /// <summary>
        /// 设置角色权限
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="permissions">权限值</param>
        /// <returns></returns>
        public ActionResult SavePermission(string roleId, string permissions)
        {
            _base_SysRoleService.SavePermission(roleId, permissions.ToList<string>());

            PermissionManage.ClearUserPermissionCache();

            return Success();
        }

        #endregion
    }
}