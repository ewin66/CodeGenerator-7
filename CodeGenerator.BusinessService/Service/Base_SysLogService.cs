using CodeGenerator.Entity.Base_SysManage;
using CodeGenerator.Util;
using System;
using System.Collections.Generic;
using CodeGenerator.BusinessService.IService;

namespace CodeGenerator.BusinessService.Base_SysManage
{
    public class Base_SysLogService : BaseService<Base_SysLog>, IBase_SysLogService
    {
        #region 外部接口

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
        public List<Base_SysLog> GetLogList(
            string logContent,
            string logType,
            string opUserName,
            DateTime? startTime,
            DateTime? endTime,
            Pagination pagination)
        {
            return LoggerFactory.GetLogger().GetLogList(logContent, logType, opUserName, startTime, endTime, pagination);
        }

        #endregion

        #region 私有成员

        #endregion

        #region 数据模型

        #endregion
    }
}