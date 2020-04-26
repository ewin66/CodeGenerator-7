using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using CodeGenerator.Util;
using CodeGenerator.Entity.Scm;
using CodeGenerator.BusinessService.IService;
using CodeGenerator.Entity.Dto;
using CodeGenerator.Entity.Enums;
using System;

namespace CodeGenerator.BusinessService.Scm
{
    public class Scm_ProductService : BaseService<Scm_Product>, IScm_ProductService
    {
       
        #region 外部接口
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="condition">查询类型</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public List<Scm_ProductDto> GetDataList(string condition, string keyword, Pagination pagination)
        {
            var q = GetIQueryable();

            //模糊查询
            if (!condition.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
                q = q.Where($@"{condition}.Contains(@0)", keyword);
            var list = q.GetPagination(pagination).ToList().MapTo<Scm_ProductDto>();

            list.ForEach(e => { e.DisableValue = EnumExtension.GetEnumDescription(((EnumWhether)Enum.ToObject(typeof(EnumWhether), e.IsDisable))); });

            return list;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="dto">dto</param>
        /// <returns></returns>
        public List<Scm_ProductDto> GetDataList(Scm_ProductDto dto)
        {
            var query = GetIQueryable().Where(f=>f.ProductNo.StartsWith(dto.Keyword) || f.ProductName.StartsWith(dto.Keyword));
            var list = query.ToList().MapTo<Scm_ProductDto>();

            return list;
        }

        

        /// <summary>
        /// 获取指定的单条数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public Scm_ProductDto GetTheData(string id)
        {
            var model = GetEntity(id).MapTo<Scm_ProductDto>(); ;
            model.DisableValue = EnumExtension.GetEnumDescription(((EnumWhether)Enum.ToObject(typeof(EnumWhether), model.IsDisable)));
            return model;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="newData">数据</param>
        public int AddData(Scm_ProductDto newData)
        {
            newData.ProductId = Guid.NewGuid().ToSequentialGuid();
            newData.CreateTime = DateTime.Now;
            newData.ProductNo = NoExtension.ShortNo(Guid.NewGuid().GetHashCode().ToString());
            var result = Insert(newData);
            if (result == 0)
                throw new Exception("添加失败！");

            return result;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        public int UpdateData(Scm_ProductDto theData)
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