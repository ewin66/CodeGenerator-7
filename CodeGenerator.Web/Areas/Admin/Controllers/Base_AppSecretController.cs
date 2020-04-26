using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CodeGenerator.BusinessService.IService;
using CodeGenerator.BusinessService.Tool;
using CodeGenerator.Util;
using CodeGenerator.Entity.Base_SysManage;

namespace CodeGenerator.Web.Areas.Admin.Controllers
{
    public class Base_AppSecretController : BaseMvcController
    {
        private IBase_AppSecretService _base_AppSecretService { get; }
        public Base_AppSecretController(IBase_AppSecretService base_AppSecretService) {
            _base_AppSecretService = base_AppSecretService;

        }

        #region 视图功能

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form(string id)
        {
            var theData = id.IsNullOrEmpty() ? new Base_AppSecret() : _base_AppSecretService.GetTheData(id);

            return View(theData);
        }

        public ActionResult PermissionForm(string appId)
        {
            ViewData["appId"] = appId;

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
            var dataList = _base_AppSecretService.GetDataList(condition, keyword, pagination);

            return Content(pagination.BuildTableResult_DataGrid(dataList).ToJson());
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="theData">保存的数据</param>
        public ActionResult SaveData(Base_AppSecret theData)
        {
            if (theData.Id.IsNullOrEmpty())
            {
                theData.Id = Guid.NewGuid().ToSequentialGuid();

                _base_AppSecretService.AddData(theData);
            }
            else
            {
                _base_AppSecretService.UpdateData(theData);
            }

            return Success();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="theData">删除的数据</param>
        public ActionResult DeleteData(string ids)
        {
            _base_AppSecretService.DeleteData(ids.ToList<string>());

            return Success("删除成功！");
        }

        public ActionResult SavePermission(string appId, string permissions)
        {
            PermissionManage.SetAppIdPermission(appId, permissions.ToList<string>());

            return Success();
        }

        #endregion
    }
}