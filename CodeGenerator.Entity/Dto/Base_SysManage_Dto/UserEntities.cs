using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CodeGenerator.Entity.Base_SysManage;

namespace CodeGenerator.Entity.Dto
{
    /// <summary>
    /// ϵͳ���û���
    /// </summary>
    public class UserEntities
    {
        /// <summary>
        /// �û�Id
        /// </summary>
        public String userId { get; set; }
        /// <summary>
        /// �û�����
        /// </summary>
        public String userName { get; set; }
        /// <summary>
        /// �û����
        /// </summary>
        public String userNo { get; set; }
        /// <summary>
        /// token
        /// </summary>
        public String token { get; set; }
        /// <summary>
        /// ��¼ʱ��
        /// </summary>
        public DateTime loginTime { get; set; }

    }
}