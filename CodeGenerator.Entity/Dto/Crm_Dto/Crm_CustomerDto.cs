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
        /// �û�����
        /// </summary>
        public string UserTypeValue { get; set; }

        /// <summary>
        /// ״̬
        /// </summary>
        public string StatusValue { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }


        
    }
}