using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeGenerator.Entity.Oms
{
    /// <summary>
    /// Oms_OrdDetail
    /// </summary>
    [Table("Oms_OrdDetail")]
    public class Oms_OrdDetail
    {

        /// <summary>
        /// 订单详情Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String OrdDetailId { get; set; }

        /// <summary>
        /// 订单明细号
        /// </summary>
        public String OrdDetailNo { get; set; }

        /// <summary>
        /// 订单Id
        /// </summary>
        public String OrderId { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public String ProductId { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public String ProductNo { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public String ProductName { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public String Specification { get; set; }

        /// <summary>
        /// 产品参数Json
        /// </summary>
        public String PoductParams { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public Decimal? Num { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public Decimal? Price { get; set; }

        /// <summary>
        /// 成本价
        /// </summary>
        public Decimal? Cost { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        public Decimal? TotalPrice { get; set; }

        /// <summary>
        /// 总成本
        /// </summary>
        public Decimal? TotalCost { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Note { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public Boolean IsDelete { get; set; }

    }
}