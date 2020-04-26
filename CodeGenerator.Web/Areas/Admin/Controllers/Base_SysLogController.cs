using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CodeGenerator.BusinessService.IService;
using CodeGenerator.Entity.Enums;
using CodeGenerator.Util;

namespace CodeGenerator.Web.Areas.Admin.Controllers
{
    public class Base_SysLogController : BaseMvcController
    {
        private IBase_SysLogService _base_SysLogService { get; }
        public Base_SysLogController(IBase_SysLogService base_SysLogService) {
            _base_SysLogService = base_SysLogService;
        }

        #region 视图功能

        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region 获取数据

        /// <summary>
        /// 获取日志列表
        /// </summary>
        /// <param name="logContent">日志内容</param>
        /// <param name="logType">日志类型</param>
        /// <param name="opUserName">操作人用户名</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="pagination">分页参数</param>
        /// <returns></returns>
        public ActionResult GetLogList(
            string logContent,
            string logType,
            string opUserName,
            DateTime? startTime,
            DateTime? endTime,
            Pagination pagination)
        {
            var dataList = _base_SysLogService.GetLogList(logContent, logType, opUserName, startTime, endTime, pagination);

            return Content(pagination.BuildTableResult_DataGrid(dataList).ToJson());
        }

        public ActionResult GetLogTypeList()
        {
            List<object> logTypeList = new List<object>();
            Enum.GetNames(typeof(EnumType.LogType)).ForEach(aName =>
            {
                logTypeList.Add(new { Name = aName, Value = aName });
            });

            return Content(logTypeList.ToJson());
        }

        #endregion

        #region 提交数据

        #endregion
    }
}