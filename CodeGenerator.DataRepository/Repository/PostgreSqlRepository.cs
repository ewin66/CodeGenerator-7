using CodeGenerator.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CodeGenerator.DataRepository
{
    public class PostgreSqlRepository : DbRepository, IRepository
    {
        #region ���캯��

        /// <summary>
        /// ���캯��
        /// </summary>
        public PostgreSqlRepository()
            : base(null, DatabaseType.PostgreSql, null)
        {
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="conStr">���ݿ�������</param>
        public PostgreSqlRepository(string conStr)
            : base(conStr, DatabaseType.PostgreSql, null)
        {
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="conStr">���ݿ�������</param>
        /// <param name="entityNamespace">ʵ�������ռ�</param>
        public PostgreSqlRepository(string conStr, string entityNamespace)
            : base(conStr, DatabaseType.PostgreSql, entityNamespace)
        {
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="dbContext">���ݿ�����������</param>
        public PostgreSqlRepository(DbContext dbContext)
            : base(dbContext, DatabaseType.PostgreSql, null)
        {
        }

        #endregion

        #region ��������

        /// <summary>
        /// ʹ��Bulk�����������ݣ��ʺϴ����������ٶȷǳ��죩
        /// </summary>
        /// <typeparam name="T">ʵ������</typeparam>
        /// <param name="entities">����</param>
        public override void BulkInsert<T>(List<T> entities)
        {
            throw new Exception("��Ǹ���ݲ�֧��PostgreSql��");
        }

        #endregion
    }
}
