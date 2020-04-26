using System;
using Microsoft.AspNetCore.Mvc;
using CodeGenerator.Util;
using CodeGenerator.Entity.Dto;
using CodeGenerator.BusinessService.IService;

namespace CodeGenerator.Web
{
    //[Area("Crm")]
    public class Crm_CusGroupController : BaseMvcController
    {
        private ICrm_CusGroupService _crm_CusGroupService { get; }
        public Crm_CusGroupController(ICrm_CusGroupService crm_CusGroupService) {
            _crm_CusGroupService = crm_CusGroupService;
        }
        #region 视图功能

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form(string id)
        {
            var theData = id.IsNullOrEmpty() ? new Crm_CusGroupDto() : _crm_CusGroupService.GetTheData(id);

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
            var dataList = _crm_CusGroupService.GetDataList(condition, keyword, pagination);

            return Content(pagination.BuildTableResult_DataGrid(dataList).ToJson());
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="theData">保存的数据</param>
        public ActionResult SaveData(Crm_CusGroupDto theData)
        {
            if(theData.GroupId.IsNullOrEmpty())
            {
                _crm_CusGroupService.AddData(theData);
            }
            else
            {
                _crm_CusGroupService.UpdateData(theData);
            }

            return Success();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="theData">删除的数据</param>
        public ActionResult DeleteData(string ids)
        {
            _crm_CusGroupService.DeleteData(ids.ToList<string>());

            return Success("删除成功！");
        }

        #endregion
    }
}