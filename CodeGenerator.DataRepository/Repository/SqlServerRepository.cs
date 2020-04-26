using CodeGenerator.Util;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CodeGenerator.DataRepository
{
    public class SqlServerRepository : DbRepository, IRepository
    {
        #region ���캯��

        /// <summary>
        /// ���캯��
        /// </summary>
        public SqlServerRepository()
            : base(null, DatabaseType.SqlServer, null)
        {
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="conStr">���ݿ�������</param>
        public SqlServerRepository(string conStr)
            : base(conStr, DatabaseType.SqlServer, null)
        {
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="conStr">���ݿ�������</param>
        /// <param name="entityNamespace">ʵ�������ռ�</param>
        public SqlServerRepository(string conStr, string entityNamespace)
            : base(conStr, DatabaseType.SqlServer, entityNamespace)
        {
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="dbContext">���ݿ�����������</param>
        public SqlServerRepository(DbContext dbContext)
            : base(dbContext, DatabaseType.SqlServer, null)
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
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = _connectionString;
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                string tableName = string.Empty;
                var tableAttribute = typeof(T).GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault();
                if (tableAttribute != null)
                    tableName = ((TableAttribute)tableAttribute).Name;
                else
                    tableName = typeof(T).Name;

                SqlBulkCopy sqlBC = new SqlBulkCopy(conn)
                {
                    BatchSize = 100000,
                    BulkCopyTimeout = 0,
                    DestinationTableName = tableName
                };
                using (sqlBC)
                {
                    sqlBC.WriteToServer(entities.ToDataTable());
                }
            }
        }

        #endregion
    }
}
