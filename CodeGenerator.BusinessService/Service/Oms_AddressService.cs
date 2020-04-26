using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using CodeGenerator.Util;
using CodeGenerator.Entity.Dto;
using CodeGenerator.Entity.Oms;
using CodeGenerator.BusinessService.IService;
using CodeGenerator.Entity.Enums;

namespace CodeGenerator.BusinessService.Oms
{
    public class Oms_AddressService : BaseService<Oms_Address>, IOms_AddressService
    {
        #region 外部接口

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="condition">查询类型</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public List<Oms_AddressDto> GetDataList(string condition, string keyword, Pagination pagination)
        {
            var q = GetIQueryable();

            //模糊查询
            if (!condition.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
                q = q.Where($@"{condition}.Contains(@0)", keyword);
            var list = q.GetPagination(pagination).ToList().MapTo<Oms_AddressDto>();
            
                    list.ForEach(e => { e.DefaultValue = EnumExtension.GetEnumDescription(((EnumWhether)Enum.ToObject(typeof(EnumWhether), e.IsDefault))); });
            return list;
        }

        /// <summary>
        /// 获取指定的单条数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public Oms_AddressDto GetTheData(string id)
        {
            var model = GetEntity(id).MapTo<Oms_AddressDto>(); ;
            return model;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="newData">数据</param>
        public int AddData(Oms_AddressDto newData)
        {
            newData.AddressId = Guid.NewGuid().ToSequentialGuid();
            newData.CreateTime = DateTime.Now;

            var result = Insert(newData);
            if (result == 0)
                throw new Exception("添加失败！");

            return result;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        public int UpdateData(Oms_AddressDto theData)
        {
            var result = Modify(theData);
            if (result == 0)
                throw new Exception("更新失败！");

            return result;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="theData">删除的数据</param>
        public int DeleteData(List<string> ids)
        {
            var result = Delete(ids);
            if (result == 0)
                throw new Exception("删除失败！");

            return result;
        }

        #endregion

        #region 私有成员

        #endregion

        #region 数据模型

        #endregion
    }
}