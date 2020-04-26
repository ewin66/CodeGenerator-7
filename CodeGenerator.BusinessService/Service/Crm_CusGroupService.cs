using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using CodeGenerator.Util;
using CodeGenerator.Entity.Dto;
using CodeGenerator.Entity.Crm;
using CodeGenerator.BusinessService.IService;
using CodeGenerator.Entity.Enums;

namespace CodeGenerator.BusinessService.Crm
{
    public class Crm_CusGroupService : BaseService<Crm_CusGroup>, ICrm_CusGroupService
    {
        #region 外部接口

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="condition">查询类型</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public List<Crm_CusGroupDto> GetDataList(string condition, string keyword, Pagination pagination)
        {
            var q = GetIQueryable();

            //模糊查询
            if (!condition.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
                q = q.Where($@"{condition}.Contains(@0)", keyword);
            var list = q.GetPagination(pagination).ToList().MapTo<Crm_CusGroupDto>();
            list.ForEach(e => {
                e.DeleteValue = EnumExtension.GetEnumDescription(((EnumWhether)Enum.ToObject(typeof(EnumWhether), e.IsDelete)));
                e.StatusName = EnumExtension.GetEnumDescription(((EnumCusGroupStatus)Enum.ToObject(typeof(EnumCusGroupStatus), e.Status)));
            });

            return list;
        }

        /// <summary>
        /// 获取指定的单条数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public Crm_CusGroupDto GetTheData(string id)
        {
            var model = GetEntity(id).MapTo<Crm_CusGroupDto>(); 
            model.StatusName = EnumExtension.GetEnumDescription(((EnumCusGroupStatus)Enum.ToObject(typeof(EnumCusGroupStatus), model.Status)));
            return model;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="newData">数据</param>
        public int AddData(Crm_CusGroupDto newData)
        {
            newData.GroupId = Guid.NewGuid().ToSequentialGuid();
            newData.CreateTime = DateTime.Now;
            newData.IsDelete = false;

            var result = Insert(newData);
            if (result == 0)
                throw new Exception("添加失败！");

            return result;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        public int UpdateData(Crm_CusGroupDto theData)
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