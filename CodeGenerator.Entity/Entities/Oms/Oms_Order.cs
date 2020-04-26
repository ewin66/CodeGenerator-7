using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeGenerator.Entity.Oms
{
    /// <summary>
    /// Oms_Order
    /// </summary>
    [Table("Oms_Order")]
    public class Oms_Order
    {

        /// <summary>
        /// 订单Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String OrderId { get; set; }

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
        /// 总数量
        /// </summary>
        public Decimal? SumNum { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public Decimal? SumPrice { get; set; }

        /// <summary>
        /// 总成本
        /// </summary>
        public Decimal? TotalCost { get; set; }

        /// <summary>
        /// 订单来源
        /// </summary>
        public Int32? Source { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public Int32? Status { get; set; }

        /// <summary>
        /// 付款渠道
        /// </summary>
        public Int32? PayChannel { get; set; }

        /// <summary>
        /// 外部单号
        /// </summary>
        public String OuterNo { get; set; }

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

        /// <summary>
        /// 是否删除
        /// </summary>
        public Boolean IsDelete { get; set; }

    }
}