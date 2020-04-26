using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Common;

namespace CodeGenerator.DataRepository
{
    public interface IRepository
    {
        #region 数据库连接相关方法

        DbContext GetDbContext();

        Action<string> HandleSqlLog { set; }

        #endregion

        #region 增加数据

        int Insert<T>(T entity) where T : class, new();
        int Insert<T>(List<T> entities) where T : class, new();
        void BulkInsert<T>(List<T> entities) where T : class, new();

        #endregion

        #region 删除数据

        int DeleteAll<T>() where T : class, new();
        int Delete<T>(string key) where T : class, new();
        int Delete<T>(List<string> keys) where T : class, new();
        int Delete<T>(T entity) where T : class, new();
        int Delete<T>(List<T> entities) where T : class, new();
        int Delete<T>(Expression<Func<T, bool>> condition) where T : class, new();

        #endregion

        #region 更新数据
        
        int Modify<T>(T entity) where T : class, new();
        int Modify<T>(List<T> entities) where T : class, new();
        int Modify<T>(T entity, params string[] proNames) where T : class, new();
        int Modify<T>(List<T> entities, params string[] proNames) where T : class, new();
        int Modify<T>(T entity, List<string> properties) where T : class, new();
        int Modify<T>(List<T> entities, List<string> properties) where T : class, new();
        int Modify<T>(Expression<Func<T, bool>> whereExpre, Action<T> set) where T : class, new();

        #endregion

        #region 查询数据

        T GetEntity<T>(params object[] keyValue) where T : class, new();
        List<T> GetList<T>() where T : class, new();
        IQueryable<T> GetIQueryable<T>() where T : class, new();
        DataTable GetDataTableWithSql(string sql);
        DataTable GetDataTableWithSql(string sql, List<DbParameter> parameters);
        List<T> GetListBySql<T>(string sqlStr) where T : class, new();
        List<T> GetListBySql<T>(string sqlStr, List<DbParameter> parameters) where T : class, new();

        #endregion

        #region 执行Sql语句

        int ExecuteSql(string sql);
        int ExecuteSql(string sql, List<DbParameter> parameters);

        #endregion 
    }
}