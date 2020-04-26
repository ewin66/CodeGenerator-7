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
    public class Oms_OrdDetailService : BaseService<Oms_OrdDetail>, IOms_OrdDetailService
    {
        static Oms_OrderService _oms_OrderService { get; } = new Oms_OrderService();


        #region 外部接口
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="condition">查询类型</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public List<Oms_OrdDetailDto> GetDataList(string condition, string keyword, Pagination pagination)
        {
            var query = from o in _oms_OrderService.GetIQueryable()
                       join d in GetIQueryable() on o.OrderId equals d.OrderId
                       where !o.IsDelete && !d.IsDelete
                        select new Oms_OrdDetailDto()
                       {
                            OrderId = o.OrderId,
                            OrderNo = o.OrderNo,
                            ContactName = o.ContactName,
                            ContactPhone = o.ContactPhone,
                            ContactAddress = o.ContactAddress,
                            Source = o.Source,
                            Status = o.Status,
                            Remark = o.Remark,
                            CreateTime = o.CreateTime,
                            PayTime = o.PayTime,
                            DeliveryTime = o.DeliveryTime,
                            FinishTime = o.FinishTime,
                            OrdDetailId = d.OrdDetailId,
                            OrdDetailNo = d.OrdDetailNo,
                            ProductNo = d.ProductNo,
                            ProductName = d.ProductName,
                            Specification = d.Specification,
                            PoductParams = d.PoductParams,
                            Num = d.Num,
                            Price = d.Price,
                            Cost = d.Cost,
                            TotalPrice = d.TotalPrice,
                            TotalCost = d.TotalCost,
                            Note = d.Note,
                            IsDelete = d.IsDelete
                        };

            //模糊查询
            if (!condition.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
                query = query.Where($@"{condition}.Contains(@0)", keyword);

            var list = query.ToList();
          
            list.ForEach(e => {
                e.DeleteValue = EnumExtension.GetEnumDescription(((EnumWhether)Enum.ToObject(typeof(EnumWhether), e.IsDelete)));
                e.SourceName = EnumExtension.GetEnumDescription(((EnumOrderSource)Enum.ToObject(typeof(EnumOrderSource), e.Source)));
                e.StatusName = EnumExtension.GetEnumDescription(((EnumOrderStatus)Enum.ToObject(typeof(EnumOrderStatus), e.Status)));
            });
            
            return list;
        }

        /// <summary>
        /// 获取指定的单条数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public Oms_OrdDetailDto GetTheData(string id)
        {
            var model = GetEntity(id).MapTo<Oms_OrdDetailDto>(); ;
            return model;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="newData">数据</param>
        public int AddData(Oms_OrdDetailDto newData)
        {
            newData.OrdDetailId = Guid.NewGuid().ToSequentialGuid();
            

            var result = Insert(newData);
            if (result == 0)
                throw new Exception("添加失败！");

            return result;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        public int UpdateData(Oms_OrdDetailDto theData)
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