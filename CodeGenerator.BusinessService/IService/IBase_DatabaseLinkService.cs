using System.Collections.Generic;
using CodeGenerator.Entity.Base_SysManage;
using CodeGenerator.Util;

namespace CodeGenerator.BusinessService.IService
{
    public interface IBase_DatabaseLinkService
    {
        #region 外部接口

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="condition">查询类型</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        List<Base_DatabaseLink> GetDataList(string condition, string keyword, Pagination pagination);

        /// <summary>
        /// 获取指定的单条数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Base_DatabaseLink GetTheData(string id);

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="newData">数据</param>
        void AddData(Base_DatabaseLink newData);

        /// <summary>
        /// 更新数据
        /// </summary>
        void UpdateData(Base_DatabaseLink theData);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="theData">删除的数据</param>
        void DeleteData(List<string> ids);

        #endregion

    }
}