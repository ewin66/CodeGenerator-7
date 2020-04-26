using CodeGenerator.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CodeGenerator.DataRepository
{
    public class DbRepository : IRepository
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="param">构造参数，可以为数据库连接字符串或者DbContext</param>
        /// <param name="dbType">数据库类型</param>
        public DbRepository(Object param, DatabaseType dbType, string entityNamespace)
        {
            BuildParam = param;
            _dbType = dbType;
            _entityNamespace = entityNamespace;
            Handle_BuildDbContext = new Func<DbContext>(() =>
            {
                return DbFactory.GetDbContext(BuildParam, _dbType, _entityNamespace);
            });
            _db = Handle_BuildDbContext?.Invoke();
            _connectionString = _db.Database.GetDbConnection().ConnectionString;
            IsDisposed = false;
        }

        #endregion

        #region 拥有成员

        /// <summary>
        /// 连接字符串
        /// </summary>
        protected string _connectionString { get; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        private DatabaseType _dbType { get; set; }

        /// <summary>
        /// 连接上下文DbContext
        /// </summary>
        private DbContext _db { get; set; }

        /// <summary>
        /// 建造DbConText所需参数
        /// </summary>
        private Object BuildParam { get; set; }

        /// <summary>
        /// 实体命名空间
        /// </summary>
        private string _entityNamespace { get; set; }

        /// <summary>
        /// 标记DbContext是否已经释放
        /// </summary>
        protected bool IsDisposed { get; set; }

        protected DbContext Db
        {
            get
            {
                if (IsDisposed)
                {
                    _db = Handle_BuildDbContext?.Invoke();
                    IsDisposed = false;
                }

                return _db;
            }
            set
            {
                _db = value;
            }
        }

        protected static PropertyInfo GetKeyProperty<T>()
        {
            return GetKeyPropertys<T>().FirstOrDefault();
        }

        protected static List<PropertyInfo> GetKeyPropertys<T>()
        {
            var properties = typeof(T)
                .GetProperties()
                .Where(x => x.GetCustomAttributes(true).Select(o => o.GetType().FullName).Contains(typeof(KeyAttribute).FullName))
                .ToList();

            return properties;
        }

        protected static string GetDbTableName<T>()
        {
            string tableName = string.Empty;
            var tableAttribute = typeof(T).GetCustomAttribute<TableAttribute>();
            if (tableAttribute != null)
                tableName = tableAttribute.Name;
            else
                tableName = typeof(T).Name;

            return tableName;
        }

        #endregion 

        #region 事件处理

        Func<DbContext> Handle_BuildDbContext { get; set; }

        public Action<string> HandleSqlLog { set => EFCoreSqlLogeerProvider.HandleSqlLog = value; }

        /// <summary>
        /// 提交到数据库
        /// </summary>
        protected int Commit()
        {
            try
            {
                int result = Db.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Db.Dispose();
                IsDisposed = true;
            }

        }

        /// <summary>
        /// 释放数据,初始化状态
        /// </summary>
        protected void Dispose()
        {
            Db?.Dispose();
            IsDisposed = true;
        }
        #endregion

        #region 数据库连接相关方法

        /// <summary>
        /// 获取DbContext
        /// </summary>
        /// <returns></returns>
        public DbContext GetDbContext()
        {
            return Db;
        }

        #endregion

        #region 增加数据

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体</param>
        public int Insert<T>(T entity) where T : class, new()
        {
            Db.Add(entity);
            return Commit();
        }

        /// <summary>
        /// 插入数据列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体列表</param>
        public int Insert<T>(List<T> entities) where T : class, new()
        {
            Db.Set<T>().AddRange(entities);
            return Commit();
        }

        /// <summary>
        /// 使用Bulk批量插入数据（适合大数据量，速度非常快）
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">数据</param>
        public virtual void BulkInsert<T>(List<T> entities) where T : class, new()
        {

        }

        #endregion

        #region 删除数据

        /// <summary>
        /// 删除表中所有数据
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        public virtual int DeleteAll<T>() where T : class, new()
        {
            TableAttribute tableAttribute = typeof(T).GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault() as TableAttribute;
            string tableName = tableAttribute.Name;
            string sql = $"DELETE {tableName}";
            return ExecuteSql(sql);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="key">主键值</param>
        public int Delete<T>(string key) where T : class, new()
        {
            T newData = new T();
            var theProperty = GetKeyProperty<T>();
            if (theProperty == null)
                throw new Exception("该实体没有主键标识！请使用[Key]标识主键！");
            var value = Convert.ChangeType(key, theProperty.PropertyType);
            theProperty.SetValue(newData, value);
            return Delete(newData);
        }

        /// <summary>
        /// 删除多条数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="keys">主键列表</param>
        public int Delete<T>(List<string> keys) where T : class, new()
        {
            var theProperty = GetKeyProperty<T>();
            if (theProperty == null)
                throw new Exception("该实体没有主键标识！请使用[Key]标识主键！");

            List<T> deleteList = new List<T>();
            keys.ForEach(aKey =>
            {
                T newData = new T();
                var value = Convert.ChangeType(aKey, theProperty.PropertyType);
                theProperty.SetValue(newData, value);
                deleteList.Add(newData);
            });

            return Delete(deleteList);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        public int Delete<T>(T entity) where T : class, new()
        {
            Db.Set<T>().Attach(entity);
            Db.Set<T>().Remove(entity);
            return Commit();
        }

        /// <summary>
        /// 删除多条数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">数据列表</param>
        public int Delete<T>(List<T> entities) where T : class, new()
        {
            foreach (var entity in entities)
            {
                Db.Set<T>().Attach(entity);
                Db.Set<T>().Remove(entity);
            }
            return Commit();
        }

        /// <summary>
        /// 通过条件删除数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="condition">条件</param>
        public virtual int Delete<T>(Expression<Func<T, bool>> condition) where T : class, new()
        {
            var deleteList = GetIQueryable<T>().Where(condition).ToList();
            return Delete(deleteList);
        }

        #endregion

        #region 更新数据

        /// <summary>
        /// 默认更新一个实体，所有字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public int Modify<T>(T entity) where T : class, new()
        {
            //对象添加到ef中
            var entry = Db.Entry<T>(entity);
            //从缓存中查询是否存在
            var obj = Db.Set<T>().Find(entry.Property(entry.CurrentValues.Properties[0].Name).CurrentValue);
            if (obj != null)
            {
                //分离缓存中的实体
                var objEntry = Db.Entry<T>(obj);
                objEntry.State = EntityState.Detached;
            }

            Db.Entry(entity).State = EntityState.Modified;

            return Commit();
        }

        /// <summary>
        /// 默认更新实体列表，所有字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        public int Modify<T>(List<T> entities) where T : class, new()
        {
            entities.ForEach(aEntity =>
            {
                //对象添加到ef中
                var entry = Db.Entry<T>(aEntity);
                //从缓存中查询是否存在
                var obj = Db.Set<T>().Find(entry.Property(entry.CurrentValues.Properties[0].Name).CurrentValue);
                if (obj != null)
                {
                    //分离缓存中的实体
                    var objEntry = Db.Entry<T>(obj);
                    objEntry.State = EntityState.Detached;
                }
                Db.Entry(aEntity).State = EntityState.Modified;
            });

            return Commit();
        }

        /// <summary>
        /// 更新一条数据,某些属性
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="proNames">需要更新的字段</param>
        public int Modify<T>(T entity, params string[] proNames) where T : class, new()
        {
            //对象添加到ef中
            var entry = Db.Entry<T>(entity);
            //从缓存中查询是否存在
            var obj = Db.Set<T>().Find(entry.Property(entry.CurrentValues.Properties[0].Name).CurrentValue);
            if (obj != null)
            {
                //分离缓存中的实体
                var objEntry = Db.Entry<T>(obj);
                objEntry.State = EntityState.Detached;
            }
            //对象状态设置为Unchanged
            entry.State = EntityState.Unchanged;
            //循环修改数组名状态
            foreach (string proName in proNames)
            {
                entry.Property(proName).IsModified = true;
            }
            //保存
            return Commit();
        }


        /// <summary>
        /// 更新多条数据,某些属性
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="proNames">需要更新的字段</param>
        public int Modify<T>(List<T> entities, params string[] proNames) where T : class, new()
        {

            entities.ForEach(entity =>
            {
                //对象添加到ef中
                var entry = Db.Entry<T>(entity);
                //从缓存中查询是否存在
                var obj = Db.Set<T>().Find(entry.Property(entry.CurrentValues.Properties[0].Name).CurrentValue);
                if (obj != null)
                {
                    //分离缓存中的实体
                    var objEntry = Db.Entry<T>(obj);
                    objEntry.State = EntityState.Detached;
                }
                //对象状态设置为Unchanged
                entry.State = EntityState.Unchanged;
                //循环修改数组名状态
                foreach (string proName in proNames)
                {
                    entry.Property(proName).IsModified = true;
                }
            });
            //保存
            return Commit();
        }

        /// <summary>
        /// 更新一条数据,某些属性
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="properties">需要更新的字段</param>
        public int Modify<T>(T entity, List<string> properties) where T : class, new()
        {
            Db.Set<T>().Attach(entity);
            properties.ForEach(aProperty =>
            {
                Db.Entry(entity).Property(aProperty).IsModified = true;
            });
            return Commit();
        }

        /// <summary>
        /// 更新多条数据,某些属性
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">数据列表</param>
        /// <param name="properties">需要更新的字段</param>
        public int Modify<T>(List<T> entities, List<string> properties) where T : class, new()
        {
            entities.ForEach(aEntity =>
            {
                Db.Set<T>().Attach(aEntity);
                properties.ForEach(aProperty =>
                {
                    Db.Entry(aEntity).Property(aProperty).IsModified = true;
                });
            });

            return Commit();
        }

        /// <summary>
        /// 指定条件更新
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="whereExpre">筛选表达式</param>
        /// <param name="set">更改属性回调</param>
        public int Modify<T>(Expression<Func<T, bool>> whereExpre, Action<T> set) where T : class, new()
        {
            var list = GetIQueryable<T>().Where(whereExpre).ToList();
            list.ForEach(aData => set(aData));
            return Modify(list);
        }

        #endregion

        #region 查询数据

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public T GetEntity<T>(params object[] keyValue) where T : class, new()
        {
            return Db.Set<T>().Find(keyValue);
        }

        /// <summary>
        /// 获取表的所有数据，当数据量很大时不要使用！
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns></returns>
        public List<T> GetList<T>() where T : class, new()
        {
            return Db.Set<T>().AsNoTracking().ToList();
        }

        /// <summary>
        /// 获取实体对应的表，延迟加载，主要用于支持Linq查询操作
        /// 注意：无缓存
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns></returns>
        public IQueryable<T> GetIQueryable<T>() where T : class, new()
        {
            return Db.Set<T>().AsNoTracking();
        }

        /// <summary>
        /// 通过Sql语句获取DataTable
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <returns></returns>
        public DataTable GetDataTableWithSql(string sql)
        {
            DbProviderFactory dbProviderFactory = DbProviderFactoryHelper.GetDbProviderFactory(_dbType);
            using (DbConnection conn = dbProviderFactory.CreateConnection())
            {
                conn.ConnectionString = _connectionString;
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = sql;

                    DbDataAdapter adapter = dbProviderFactory.CreateDataAdapter();
                    adapter.SelectCommand = cmd;
                    DataSet table = new DataSet();
                    adapter.Fill(table);

                    return table.Tables[0];
                }
            }
        }

        /// <summary>
        /// 通过Sql参数查询返回DataTable
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="parameters">查询参数</param>
        /// <returns></returns>
        public DataTable GetDataTableWithSql(string sql, List<DbParameter> parameters)
        {
            DbProviderFactory dbProviderFactory = DbProviderFactoryHelper.GetDbProviderFactory(_dbType);
            using (DbConnection conn = dbProviderFactory.CreateConnection())
            {
                conn.ConnectionString = _connectionString;
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = sql;

                    if (parameters != null && parameters.Count > 0)
                    {
                        foreach (var item in parameters)
                        {
                            cmd.Parameters.Add(item);
                        }
                    }

                    DbDataAdapter adapter = dbProviderFactory.CreateDataAdapter();
                    adapter.SelectCommand = cmd;
                    DataSet table = new DataSet();
                    adapter.Fill(table);
                    cmd.Parameters.Clear();

                    return table.Tables[0];
                }
            }
        }

        /// <summary>
        /// 通过sql返回List
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sqlStr">sql语句</param>
        /// <returns></returns>
        public List<T> GetListBySql<T>(string sqlStr) where T : class, new()
        {
            return GetDataTableWithSql(sqlStr).ToList<T>();
        }

        /// <summary>
        /// 通过sql返回list
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="sqlStr">sql语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public List<T> GetListBySql<T>(string sqlStr, List<DbParameter> parameters) where T : class, new()
        {
            return GetDataTableWithSql(sqlStr, parameters).ToList<T>();
        }

        #endregion

        #region 执行Sql语句

        /// <summary>
        /// 执行Sql语句
        /// </summary>
        /// <param name="sql">Sql语句</param>
        public int ExecuteSql(string sql)
        {
            int result = Db.Database.ExecuteSqlCommand(sql);
            Dispose();
            return result;
        }

        /// <summary>
        /// 通过参数执行Sql语句
        /// </summary>
        /// <param name="sql">Sql语句</param>
        public int ExecuteSql(string sql, List<DbParameter> parameters)
        {

            int result = Db.Database.ExecuteSqlCommand(sql, parameters.ToArray());
            Dispose();
            return result;
        }

        #endregion

        #region 检验语句
        //public bool Exists(T entity)
        //{
        //    var _ObjContext = ((IObjectContextAdapter)this._context).ObjectContext;
        //    var _ObjSet = _ObjContext.CreateObjectSet<T>();

        //    var entityKey = _ObjContext.CreateEntityKey(_ObjSet.EntitySet.Name, entity);

        //    Object foundEntity;
        //    var exists = _ObjContext.TryGetObjectByKey(entityKey, out foundEntity);
        //    // TryGetObjectByKey attaches a found entity
        //    // Detach it here to prevent side-effects
        //    if (exists)
        //    {
        //        _ObjContext.Detach(foundEntity);
        //    }

        //    return (exists);
        //}
        #endregion

    }
}