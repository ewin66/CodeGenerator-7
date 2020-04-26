using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

namespace CodeGenerator.BusinessService
{
    public interface IBaseService<T> where T : class, new()
    {

        #region 增加数据

        int Insert(T entity);
        int Insert(List<T> entities);
        void BulkInsert(List<T> entities);

        #endregion

        #region 删除数据

        int DeleteAll();
        int Delete(string key);
        int Delete(List<string> keys);
        int Delete(T entity);
        int Delete(List<T> entities);
        int Delete(Expression<Func<T, bool>> condition);

        #endregion

        #region 更新数据

        int Modify(List<T> entities);

        int Modify(T entity, params string[] proNames);
        int Modify(List<T> entities, params string[] proNames);

        int Modify(T entity, List<string> properties);
        int Modify(List<T> entities, List<string> properties);

        #endregion

        #region 查询数据

        T GetEntity(params object[] keyValue);
        List<T> GetList();
        IQueryable<T> GetIQueryable();
        DataTable GetDataTableWithSql(string sql);
        DataTable GetDataTableWithSql(string sql, List<DbParameter> parameters);
        List<U> GetListBySql<U>(string sqlStr) where U : class, new();
        List<U> GetListBySql<U>(string sqlStr, List<DbParameter> param) where U : class, new();

        #endregion

        #region 执行Sql语句

        int ExecuteSql(string sql);
        void ExecuteSql(string sql, List<DbParameter> spList);

        #endregion
    }
}
