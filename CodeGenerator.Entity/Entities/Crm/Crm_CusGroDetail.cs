using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeGenerator.Entity.Crm
{
    /// <summary>
    /// Crm_CusGroDetail
    /// </summary>
    [Table("Crm_CusGroDetail")]
    public class Crm_CusGroDetail
    {

        /// <summary>
        /// 报价组详情Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String CusGroDetailId { get; set; }

        /// <summary>
        /// 报价组Id
        /// </summary>
        public String GroupId { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public String ProductId { get; set; }

        /// <summary>
        /// 售价
        /// </summary>
        public Decimal? Price { get; set; }

        /// <summary>
        /// 销售状态
        /// </summary>
        public Int32? SaleStatus { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? LastModifyTime { get; set; }

        /// <summary>
        /// 最后修改Id
        /// </summary>
        public String LastModifyId { get; set; }

    }
}