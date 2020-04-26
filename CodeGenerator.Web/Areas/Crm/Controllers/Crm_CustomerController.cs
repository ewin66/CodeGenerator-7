using System;
using Microsoft.AspNetCore.Mvc;
using CodeGenerator.Util;
using CodeGenerator.Entity.Dto;
using CodeGenerator.BusinessService.IService;

namespace CodeGenerator.Web
{
    //[Area("Crm")]
    public class Crm_CustomerController : BaseMvcController
    {
        private ICrm_CustomerService _crm_CustomerService { get; }
        public Crm_CustomerController(ICrm_CustomerService crm_CustomerService) {
            _crm_CustomerService = crm_CustomerService;
        }
        #region 视图功能

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form(string id)
        {
            var theData = id.IsNullOrEmpty() ? new Crm_CustomerDto() : _crm_CustomerService.GetTheData(id);

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
            var dataList = _crm_CustomerService.GetDataList(condition, keyword, pagination);

            return Content(pagination.BuildTableResult_DataGrid(dataList).ToJson());
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="theData">保存的数据</param>
        public ActionResult SaveData(Crm_CustomerDto theData)
        {
            if(theData.CustomerId.IsNullOrEmpty())
            {
                _crm_CustomerService.AddData(theData);
            }
            else
            {
                _crm_CustomerService.UpdateData(theData);
            }

            return Success();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="theData">删除的数据</param>
        public ActionResult DeleteData(string ids)
        {
            _crm_CustomerService.DeleteData(ids.ToList<string>());

            return Success("删除成功！");
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="theData">删除的数据</param>
        public ActionResult CustomerLogin(Crm_CustomerDto theData)
        {
            theData = _crm_CustomerService.CustomerLogin(theData);

            return Success("登录成功！", theData);
        }

        #endregion
    }
}