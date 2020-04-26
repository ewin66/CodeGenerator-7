using System;
using Microsoft.AspNetCore.Mvc;
using CodeGenerator.Util;
using CodeGenerator.Entity.Scm;
using CodeGenerator.BusinessService.IService;
using CodeGenerator.Entity.Dto;
using System.Linq;

namespace CodeGenerator.Web
{
    //[Area("Scm")]
    public class Scm_ProductController : BaseMvcController
    {
        private IScm_ProductService _scm_ProductService { get; }
        public Scm_ProductController(IScm_ProductService scm_ProductService) {
            _scm_ProductService = scm_ProductService;
        }
        #region 视图功能

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form(string id)
        {
            var theData = id.IsNullOrEmpty() ? new Scm_ProductDto() : _scm_ProductService.GetTheData(id);

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
            var dataList = _scm_ProductService.GetDataList(condition, keyword, pagination);

            return Content(pagination.BuildTableResult_DataGrid(dataList).ToJson());
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="condition">查询类型</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>o
        public ActionResult SearchDataList(Scm_ProductDto dto)
        {
            var dataList = _scm_ProductService.GetDataList(dto);

            //return ResultObject(dataList);
            return Success(dataList);
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="theData">保存的数据</param>
        public ActionResult SaveData(Scm_ProductDto theData)
        {
            if(theData.ProductId.IsNullOrEmpty())
            {
                _scm_ProductService.AddData(theData);
            }
            else
            {
                _scm_ProductService.UpdateData(theData);
            }

            return Success();
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="theData">删除的数据</param>
        public ActionResult DeleteData(string ids)
        {
            _scm_ProductService.DeleteData(ids.ToList<string>());

            return Success("删除成功！");
        }

        #endregion
    }
}