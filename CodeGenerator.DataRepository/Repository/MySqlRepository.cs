using CodeGenerator.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Text;
using System.Linq;

namespace CodeGenerator.DataRepository
{
    public class MySqlRepository : DbRepository, IRepository
    {
        #region ���캯��

        /// <summary>
        /// ���캯��
        /// </summary>
        public MySqlRepository()
            : base(null, DatabaseType.MySql, null)
        {
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="conStr">���ݿ�������</param>
        public MySqlRepository(string conStr)
            : base(conStr, DatabaseType.MySql, null)
        {
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="conStr">���ݿ�������</param>
        /// <param name="entityNamespace">ʵ�������ռ�</param>
        public MySqlRepository(string conStr, string entityNamespace)
            : base(conStr, DatabaseType.MySql, entityNamespace)
        {
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="dbContext">���ݿ�����������</param>
        public MySqlRepository(DbContext dbContext)
            : base(dbContext, DatabaseType.MySql, null)
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
            DataTable dt = entities.ToDataTable();
            using (MySqlConnection conn = new MySqlConnection())
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

                int insertCount = 0;
                string tmpPath = Path.Combine(Path.GetTempPath(), DateTime.Now.ToCstTime().Ticks.ToString() + "_" + Guid.NewGuid().ToString() + ".tmp");
                string csv = dt.ToCsvStr();
                File.WriteAllText(tmpPath, csv, Encoding.UTF8);

                using (MySqlTransaction tran = conn.BeginTransaction())
                {
                    MySqlBulkLoader bulk = new MySqlBulkLoader(conn)
                    {
                        FieldTerminator = ",",
                        FieldQuotationCharacter = '"',
                        EscapeCharacter = '"',
                        LineTerminator = "\r\n",
                        FileName = tmpPath,
                        NumberOfLinesToSkip = 0,
                        TableName = tableName,
                    };
                    try
                    {
                        bulk.Columns.AddRange(dt.Columns.Cast<DataColumn>().Select(colum => colum.ColumnName).ToList());
                        insertCount = bulk.Load();
                        tran.Commit();
                    }
                    catch (MySqlException ex)
                    {
                        if (tran != null)
                            tran.Rollback();

                        throw ex;
                    }
                }
                File.Delete(tmpPath);
            }
        }

        #endregion

        #region ɾ������

        public override int DeleteAll<T>()
        {
            return Delete(GetList<T>());
        }

        #endregion
    }
}
