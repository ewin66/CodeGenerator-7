using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CodeGenerator.BusinessService.IService;
using CodeGenerator.Util;

namespace CodeGenerator.Web.Areas.Admin.Controllers
{
    public class RapidDevelopmentController : BaseMvcController
    {
        private IRapidDevelopmentService _rapidDevelopmentService { get; }
        public RapidDevelopmentController(IRapidDevelopmentService rapidDevelopmentService) {
            _rapidDevelopmentService = rapidDevelopmentService;
        }

        #region 视图功能

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Form()
        {
            return View();
        }

        #endregion

        #region 获取数据

        /// <summary>
        /// 获取所有数据库连接
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllDbLink()
        {
            var dataList = _rapidDevelopmentService.GetAllDbLink();

            return Content(dataList.ToJson());
        }

        /// <summary>
        /// 获取数据库所有表
        /// </summary>
        /// <param name="linkId">数据库连接Id</param>
        /// <returns></returns>
        public ActionResult GetDbTableList(string linkId)
        {
            Pagination pagination = new Pagination
            {
                PageIndex = 1,
                PageRows = int.MaxValue,
                RecordCount = int.MaxValue
            };

            return Content(pagination.BuildTableResult_DataGrid(_rapidDevelopmentService.GetDbTableList(linkId)).ToJson());
        }

        #endregion

        #region 提交数据

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="linkId">连接Id</param>
        /// <param name="areaName">区域名</param>
        /// <param name="tables">表列表</param>
        /// <param name="buildType">需要生成类型</param>
        /// <param name="project">需要生成类型</param>
        /// <param name="path">需要生成类型</param>
        public ActionResult BuildCode(string linkId, string areaName, string tables, string buildType, string project, string path)
        {
            _rapidDevelopmentService.BuildCode(linkId, areaName, tables, buildType, project, path);

            return Success("生成成功！");
        }

        #endregion
    }
}