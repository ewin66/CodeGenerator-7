using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CodeGenerator.BusinessService.IService;
using CodeGenerator.Util;
using CodeGenerator.Entity.Base_SysManage;

namespace CodeGenerator.Web.Areas.Admin.Controllers
{
    public class Base_DatabaseLinkController : BaseMvcController
    {
        private IBase_DatabaseLinkService _base_DatabaseLinkService { get; }
        public Base_DatabaseLinkController(IBase_DatabaseLinkService base_DatabaseLinkService) {
            _base_DatabaseLinkService = base_DatabaseLinkService;
        }



        #region 视图功能

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form(string id)
        {
            var theData = id.IsNullOrEmpty() ? new Base_DatabaseLink() : _base_DatabaseLinkService.GetTheData(id);

            return View(theData);
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
            var dataList = _base_DatabaseLinkService.GetDataList(condition, keyword, pagination);

            return Content(pagination.BuildTableResult_DataGrid(dataList).ToJson());
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="theData">保存的数据</param>
        public ActionResult SaveData(Base_DatabaseLink theData)
        {
            if (theData.Id.IsNullOrEmpty())
            {
                theData.Id = Guid.NewGuid().ToSequentialGuid();

                _base_DatabaseLinkService.AddData(theData);
            }
            else
            {
                _base_DatabaseLinkService.UpdateData(theData);
            }

            return Success();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="theData">删除的数据</param>
        public ActionResult DeleteData(string ids)
        {
            _base_DatabaseLinkService.DeleteData(ids.ToList<string>());

            return Success("删除成功！");
        }

        #endregion
    }
}