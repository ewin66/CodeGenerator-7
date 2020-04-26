using CodeGenerator.Entity.Base_SysManage;
using CodeGenerator.Util;
using System;
using System.Collections.Generic;

namespace CodeGenerator.BusinessService
{
    interface ILogger
    {
        void WriteSysLog(Base_SysLog log);
        List<Base_SysLog> GetLogList(
            string logContent,
            string logType,
            string opUserName,
            DateTime? startTime,
            DateTime? endTime,
            Pagination pagination);
    }
}
