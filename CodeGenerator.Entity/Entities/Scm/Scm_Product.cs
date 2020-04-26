using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeGenerator.Entity.Scm
{
    /// <summary>
    /// Scm_Product
    /// </summary>
    [Table("Scm_Product")]
    public class Scm_Product
    {

        /// <summary>
        /// 产品Id
        /// </summary>
        [Key]
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
        /// 成本价
        /// </summary>
        public Decimal? CostPrice { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public Int32? Sort { get; set; }

        /// <summary>
        /// 单位规格
        /// </summary>
        public String Specification { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>
        public String Brand { get; set; }

        /// <summary>
        /// 产地
        /// </summary>
        public String Place { get; set; }

        /// <summary>
        /// 净重
        /// </summary>
        public String Weight { get; set; }

        /// <summary>
        /// 存储条件
        /// </summary>
        public String Conditions { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        public Boolean IsDisable { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

    }
}