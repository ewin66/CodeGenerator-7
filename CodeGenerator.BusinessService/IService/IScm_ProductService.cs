using System.Collections.Generic;
using CodeGenerator.Entity.Dto;
using CodeGenerator.Entity.Scm;
using CodeGenerator.Util;

namespace CodeGenerator.BusinessService.IService
{
    public interface IScm_ProductService
    {
        #region 外部接口

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="condition" >查询类型</param>
        /// <param name="keyword" >关键字</param>
        /// <returns></returns>
        List<Scm_ProductDto> GetDataList(string condition, string keyword, Pagination pagination);

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="dto" >dto</param>
        /// <returns></returns>
        List<Scm_ProductDto> GetDataList(Scm_ProductDto dto);

        /// <summary>
        /// 获取指定的单条数据
        /// </summary>
        /// <param name="id" >主键</param>
        /// <returns></returns>
        Scm_ProductDto GetTheData(string id);


        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="newData" >数据</param>
        int AddData(Scm_ProductDto newData);

        /// <summary>
        /// 更新数据
        /// </summary>
        int UpdateData(Scm_ProductDto theData);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="theData" >删除的数据</param>
        int DeleteData(List<string> ids);
        #endregion
       
    }     
}