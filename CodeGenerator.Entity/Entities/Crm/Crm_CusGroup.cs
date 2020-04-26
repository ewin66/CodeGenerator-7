using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeGenerator.Entity.Crm
{
    /// <summary>
    /// Crm_CusGroup
    /// </summary>
    [Table("Crm_CusGroup")]
    public class Crm_CusGroup
    {

        /// <summary>
        /// 客户组Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String GroupId { get; set; }

        /// <summary>
        /// 客户组名称
        /// </summary>
        public String GroupName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public Int32? Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public String CreatorId { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public Boolean? IsDelete { get; set; }

    }
}