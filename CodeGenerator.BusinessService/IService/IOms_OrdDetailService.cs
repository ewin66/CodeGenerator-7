
using System.Collections.Generic;
using CodeGenerator.Util;
using CodeGenerator.Entity.Dto;

namespace CodeGenerator.BusinessService.IService
{
    public interface IOms_OrdDetailService
    {
        #region 外部接口

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="condition" >查询类型</param>
        /// <param name="keyword" >关键字</param>
        /// <returns></returns>
        List<Oms_OrdDetailDto> GetDataList(string condition, string keyword, Pagination pagination);

        /// <summary>
        /// 获取指定的单条数据
        /// </summary>
        /// <param name="id" >主键</param>
        /// <returns></returns>
        Oms_OrdDetailDto GetTheData(string id);


        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="newData" >数据</param>
        int AddData(Oms_OrdDetailDto newData);

        /// <summary>
        /// 更新数据
        /// </summary>
        int UpdateData(Oms_OrdDetailDto theData);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="theData" >删除的数据</param>
        int DeleteData(List<string> ids);
        #endregion
       
    }     
}