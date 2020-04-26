using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using CodeGenerator.Util;
using CodeGenerator.Entity.Dto;
using CodeGenerator.Entity.Scm;
using CodeGenerator.Entity.Crm;
using CodeGenerator.BusinessService.IService;
using CodeGenerator.BusinessService.Scm;
using CodeGenerator.Entity.Enums;
using CodeGenerator.BusinessService.Common;

namespace CodeGenerator.BusinessService.Crm
{
    public class Crm_CusGroDetailService : BaseService<Crm_CusGroDetail>, ICrm_CusGroDetailService
    {
        static Scm_ProductService _scm_ProductService { get; } = new Scm_ProductService();

        #region 外部接口
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="condition">查询类型</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public List<Crm_CusGroDetailDto> GetDataList(string condition, string keyword, string groupId, Pagination pagination)
        {
            var query = from d in GetIQueryable() 
                        join p in _scm_ProductService.GetIQueryable() on d.ProductId equals p.ProductId
                        where d.GroupId.Equals(groupId)
                        select new Crm_CusGroDetailDto()
                        {
                            CusGroDetailId = d.CusGroDetailId,
                            GroupId = d.GroupId,
                            ProductId = d.ProductId,
                            Price = d.Price,
                            SaleStatus = d.SaleStatus,
                            LastModifyTime = d.LastModifyTime,
                            LastModifyId = d.LastModifyId,
                            ProductNo = p.ProductNo,
                            ProductName = p.ProductName,
                            CostPrice = p.CostPrice,
                            Specification = p.Specification,
                            IsDisable = p.IsDisable,
                        };

            //模糊查询
            if (!condition.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
                query = query.Where($@"{condition}.Contains(@0)", keyword);

            var list = query.ToList();

            list.ForEach(e => {
                e.DisableValue = EnumExtension.GetEnumDescription(((EnumWhether)Enum.ToObject(typeof(EnumWhether), e.IsDisable)));
                e.SaleStatusValue = EnumExtension.GetEnumDescription(((EnumGroupDetailStatus)Enum.ToObject(typeof(EnumGroupDetailStatus), e.SaleStatus)));
            });

            return list;
         
        }

        /// <summary>
        /// 获取指定的单条数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public Crm_CusGroDetailDto GetTheData(string id)
        {
            var model = GetEntity(id).MapTo<Crm_CusGroDetailDto>();
            model.ProductName = _scm_ProductService.GetEntity(model.ProductId).ProductName;

            model.DisableValue = EnumExtension.GetEnumDescription(((EnumWhether)Enum.ToObject(typeof(EnumWhether), model.IsDisable)));
            model.SaleStatusValue = EnumExtension.GetEnumDescription(((EnumGroupDetailStatus)Enum.ToObject(typeof(EnumGroupDetailStatus), model.SaleStatus)));


            return model;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="newData">数据</param>
        public int AddData(Crm_CusGroDetailDto newData)
        {
            newData.CusGroDetailId = Guid.NewGuid().ToSequentialGuid();
            newData.LastModifyId = Operator.UserId;
            newData.LastModifyTime = DateTime.Now;

            var result = Insert(newData);
            if (result == 0)
                throw new Exception("添加失败！");

            return result;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        public int UpdateData(Crm_CusGroDetailDto theData)
        {
            theData.LastModifyTime = DateTime.Now;
            theData.LastModifyId = Operator.UserId;
            
            var result = Modify(theData, "SaleStatus", "Price", "LastModifyTime", "LastModifyId");
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