﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CodeGenerator.BusinessService.Tool;
using CodeGenerator.Entity.Base_SysManage;
using CodeGenerator.BusinessService.IService;
using CodeGenerator.Util;


namespace CodeGenerator.Web.Areas.Admin.Controllers
{
    public class Base_UserController : BaseMvcController
    {
        private IBase_UserService _base_UserService { get; }

        public Base_UserController(IBase_UserService base_UserService)
        {
            _base_UserService = base_UserService;
        }


        #region 视图功能

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form(string id)
        {
            var theData = id.IsNullOrEmpty() ? new Base_User() : _base_UserService.GetTheData(id);

            return View(theData);
        }

        public ActionResult ChangePwdForm()
        {
            return View();
        }

        public ActionResult PermissionForm(string userId)
        {
            ViewData["userId"] = userId;

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
            var dataList = _base_UserService.GetDataList(condition, keyword, pagination);

            return Content(pagination.BuildTableResult_DataGrid(dataList).ToJson());
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="theData">保存的数据</param>
        public ActionResult SaveData(Base_User theData, string Pwd, string RoleIdList)
        {
            if (!Pwd.IsNullOrEmpty())
                theData.Password = Pwd.ToMD5String();
            var roleIdList = RoleIdList.ToList<string>();

            if (theData.Id.IsNullOrEmpty())
            {
                theData.Id = Guid.NewGuid().ToSequentialGuid();
                theData.UserId = Guid.NewGuid().ToSequentialGuid();

                _base_UserService.AddData(theData);
            }
            else
            {
                _base_UserService.UpdateData(theData);
            }

            _base_UserService.SetUserRole(theData.UserId, roleIdList);

            return Success();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="theData">删除的数据</param>
        public ActionResult DeleteData(string ids)
        {
            _base_UserService.DeleteData(ids.ToList<string>());

            return Success("删除成功！");
        }

        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="oldPwd">老密码</param>
        /// <param name="newPwd">新密码</param>
        public ActionResult ChangePwd(string oldPwd, string newPwd)
        {
            var res = _base_UserService.ChangePwd(oldPwd, newPwd);

            return Content(res.ToJson());
        }

        /// <summary>
        /// 保存权限
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="permissions">权限</param>
        /// <returns></returns>
        public ActionResult SavePermission(string userId, string permissions)
        {
            PermissionManage.SetUserPermission(userId, permissions.ToList<string>());

            return Success();
        }

        #endregion
    }
}