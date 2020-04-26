using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CodeGenerator.Entity.Crm;

namespace CodeGenerator.Entity.Dto
{
    /// <summary>
    /// Crm_CusGroDetail
    /// </summary>
    public class Crm_CusGroDetailDto:Crm_CusGroDetail
    {
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
        /// 单位规格
        /// </summary>
        public String Specification { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        public Boolean IsDisable { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        public string DisableValue { get; set; }

        /// <summary>
        /// 销售状态
        /// </summary>
        public string SaleStatusValue { get; set; }


    }
}