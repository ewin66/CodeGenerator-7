using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeGenerator.Entity.Crm
{
    /// <summary>
    /// Crm_Customer
    /// </summary>
    [Table("Crm_Customer")]
    public class Crm_Customer
    {

        /// <summary>
        /// 客户Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String CustomerId { get; set; }

        /// <summary>
        /// 客户编号
        /// </summary>
        public String No { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 绑定手机号
        /// </summary>
        public String Phone { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public String Password { get; set; }

        /// <summary>
        /// 支付密码
        /// </summary>
        public String PayPwd { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public Decimal? Balance { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public Int32 UserType { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public Int32 Status { get; set; }

        /// <summary>
        /// 微信Id
        /// </summary>
        public String OpenId { get; set; }

        /// <summary>
        /// 微信昵称
        /// </summary>
        public String NickName { get; set; }

        /// <summary>
        /// 微信头像
        /// </summary>
        public String Headimgurl { get; set; }

        /// <summary>
        /// 业务员
        /// </summary>
        public String SalesId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 报价组Id
        /// </summary>
        public String PriceGroupId { get; set; }

    }
}