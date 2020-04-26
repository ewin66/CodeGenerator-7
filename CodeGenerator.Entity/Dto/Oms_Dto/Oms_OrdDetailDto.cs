using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CodeGenerator.Entity.Oms;

namespace CodeGenerator.Entity.Dto
{
    /// <summary>
    /// Oms_OrdDetail
    /// </summary>
    public class Oms_OrdDetailDto:Oms_OrdDetail
    {
        /// <summary>
        /// 是否删除
        /// </summary>
        public string DeleteValue { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public String OrderNo { get; set; }

        /// <summary>
        /// 客户Id
        /// </summary>
        public String CustomerId { get; set; }


        /// <summary>
        /// 收货人姓名
        /// </summary>
        public String ContactName { get; set; }

        /// <summary>
        /// 收货电话
        /// </summary>
        public String ContactPhone { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        public String ContactAddress { get; set; }


        /// <summary>
        /// 订单来源
        /// </summary>
        public Int32? Source { get; set; }

        /// <summary>
        /// 订单来源
        /// </summary>
        public String SourceName { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public Int32? Status { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public String StatusName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Remark { get; set; }


        /// <summary>
        /// 创建人Id
        /// </summary>
        public String CreateId { get; set; }



        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 付款时间
        /// </summary>
        public DateTime? PayTime { get; set; }

        /// <summary>
        /// 发货时间
        /// </summary>
        public DateTime? DeliveryTime { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? FinishTime { get; set; }






    }
}