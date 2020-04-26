using System;
using Microsoft.AspNetCore.Mvc;
using CodeGenerator.Util;
using CodeGenerator.Entity.Dto;
using CodeGenerator.BusinessService.IService;

namespace CodeGenerator.Web
{
    //[Area("Crm")]
    public class Crm_CusGroDetailController : BaseMvcController
    {
        private ICrm_CusGroDetailService _crm_CusGroDetailService { get; }
        public Crm_CusGroDetailController(ICrm_CusGroDetailService crm_CusGroDetailService) {
            _crm_CusGroDetailService = crm_CusGroDetailService;
        }
        #region 视图功能

        public ActionResult Index(string groupId)
        {
            ViewBag.groupId = groupId;
            return View();
        }

        public ActionResult Form(Crm_CusGroDetailDto model)
        {
            model = model.CusGroDetailId.IsNullOrEmpty() ? model : _crm_CusGroDetailService.GetTheData(model.CusGroDetailId);


            return View(model);
        }

        #endregion

        #region 获取数据

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="condition">查询类型</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public ActionResult GetDataList(string condition, string keyword, string groupId, Pagination pagination)
        {
            var dataList = _crm_CusGroDetailService.GetDataList(condition, keyword, groupId, pagination);

            return Content(pagination.BuildTableResult_DataGrid(dataList).ToJson());
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="theData">保存的数据</param>
        public ActionResult SaveData(Crm_CusGroDetailDto theData)
        {
            if(theData.CusGroDetailId.IsNullOrEmpty())
            {
                _crm_CusGroDetailService.AddData(theData);
            }
            else
            {
                _crm_CusGroDetailService.UpdateData(theData);
            }

            return Success();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="theData">删除的数据</param>
        public ActionResult DeleteData(string ids)
        {
            _crm_CusGroDetailService.DeleteData(ids.ToList<string>());

            return Success("删除成功！");
        }

        #endregion
    }
}