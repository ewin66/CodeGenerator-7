using CodeGenerator.Entity.Base_SysManage;
using CodeGenerator.Util;
using System;
using System.Collections.Generic;
using CodeGenerator.BusinessService.IService;

namespace CodeGenerator.BusinessService.Base_SysManage
{
    public class Base_SysLogService : BaseService<Base_SysLog>, IBase_SysLogService
    {
        #region �ⲿ�ӿ�

        /// <summary>
        /// ��ȡ��־�б�
        /// </summary>
        /// <param name="logContent">��־����</param>
        /// <param name="logType">��־����</param>
        /// <param name="opUserName">�������û���</param>
        /// <param name="startTime">��ʼʱ��</param>
        /// <param name="endTime">����ʱ��</param>
        /// <param name="pagination">��ҳ����</param>
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

        #region ˽�г�Ա

        #endregion

        #region ����ģ��

        #endregion
    }
}