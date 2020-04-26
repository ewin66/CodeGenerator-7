using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CodeGenerator.Entity.Base_SysManage;

namespace CodeGenerator.Entity.Dto
{
    /// <summary>
    /// 系统，用户表
    /// </summary>
    public class UserEntities
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public String userId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public String userName { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public String userNo { get; set; }
        /// <summary>
        /// token
        /// </summary>
        public String token { get; set; }
        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime loginTime { get; set; }

    }
}