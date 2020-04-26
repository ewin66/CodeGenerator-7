using System;
using Microsoft.AspNetCore.Mvc;
using CodeGenerator.Util;
using CodeGenerator.Entity.Dto;
using CodeGenerator.BusinessService.IService;

namespace CodeGenerator.Web
{
    //[Area("Oms")]
    public class Oms_OrdDetailController : BaseMvcController
    {
        private IOms_OrdDetailService _oms_OrdDetailService { get; }
        public Oms_OrdDetailController(IOms_OrdDetailService oms_OrdDetailService) {
            _oms_OrdDetailService = oms_OrdDetailService;
        }
        #region 视图功能

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form(string id)
        {
            var theData = id.IsNullOrEmpty() ? new Oms_OrdDetailDto() : _oms_OrdDetailService.GetTheData(id);

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
            var dataList = _oms_OrdDetailService.GetDataList(condition, keyword, pagination);

            return Content(pagination.BuildTableResult_DataGrid(dataList).ToJson());
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="theData">保存的数据</param>
        public ActionResult SaveData(Oms_OrdDetailDto theData)
        {
            if(theData.OrdDetailId.IsNullOrEmpty())
            {
                _oms_OrdDetailService.AddData(theData);
            }
            else
            {
                _oms_OrdDetailService.UpdateData(theData);
            }

            return Success();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="theData">删除的数据</param>
        public ActionResult DeleteData(string ids)
        {
            _oms_OrdDetailService.DeleteData(ids.ToList<string>());

            return Success("删除成功！");
        }

        #endregion
    }
}