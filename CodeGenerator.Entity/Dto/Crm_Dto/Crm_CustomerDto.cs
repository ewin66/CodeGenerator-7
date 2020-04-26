using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CodeGenerator.Entity.Crm;

namespace CodeGenerator.Entity.Dto
{
    /// <summary>
    /// Crm_Customer
    /// </summary>
    public class Crm_CustomerDto:Crm_Customer
    {
        /// <summary>
        /// 用户类型
        /// </summary>
        public string UserTypeValue { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string StatusValue { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }


        
    }
}