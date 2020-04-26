
using System.Collections.Generic;
using CodeGenerator.Util;
using CodeGenerator.Entity.Dto;

namespace CodeGenerator.BusinessService.IService
{
    public interface ICrm_CustomerService
    {
        #region 外部接口

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="condition" >查询类型</param>
        /// <param name="keyword" >关键字</param>
        /// <returns></returns>
        List<Crm_CustomerDto> GetDataList(string condition, string keyword, Pagination pagination);

        /// <summary>
        /// 获取指定的单条数据
        /// </summary>
        /// <param name="id" >主键</param>
        /// <returns></returns>
        Crm_CustomerDto GetTheData(string id);


        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="newData" >数据</param>
        int AddData(Crm_CustomerDto newData);

        /// <summary>
        /// 更新数据
        /// </summary>
        int UpdateData(Crm_CustomerDto theData);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="theData" >删除的数据</param>
        int DeleteData(List<string> ids);

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="theData">登录信息</param>
        Crm_CustomerDto CustomerLogin(Crm_CustomerDto dto);


        /// <summary>
        /// 获取用户类型名称
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Crm_CustomerDto GetUserTypeName(Crm_CustomerDto dto);

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        dynamic GetJwtToken3(Crm_CustomerDto dto);

        /// <summary>
        /// 更新Token
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        dynamic RefreshToken(Crm_CustomerDto dto);

        

        #endregion

    }     
}