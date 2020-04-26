using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CodeGenerator.Entity.Crm;

namespace CodeGenerator.Entity.Dto
{
    /// <summary>
    /// Crm_CusGroup
    /// </summary>
    public class Crm_CusGroupDto:Crm_CusGroup
    {

        /// <summary>
        /// 是否删除
        /// </summary>
        public string DeleteValue { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public String StatusName { get; set; }

    }
}