using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeGenerator.Entity.Oms
{
    /// <summary>
    /// Oms_Address
    /// </summary>
    [Table("Oms_Address")]
    public class Oms_Address
    {

        /// <summary>
        /// 地址Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String AddressId { get; set; }

        /// <summary>
        /// 客户Id
        /// </summary>
        public String CustomerId { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public String Phone { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public String Province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public String City { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        public String Area { get; set; }

        /// <summary>
        /// 街道
        /// </summary>
        public String Street { get; set; }

        /// <summary>
        /// 地点
        /// </summary>
        public String Site { get; set; }

        /// <summary>
        /// 是否默认
        /// </summary>
        public Boolean? IsDefault { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

    }
}