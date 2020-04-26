using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeGenerator.Entity.Base_SysManage
{
    /// <summary>
    /// �û�Ȩ�ޱ�
    /// </summary>
    [Table("Base_PermissionUser")]
    public class Base_PermissionUser
    {

        /// <summary>
        /// ��������
        /// </summary>
        [Key]
        public String Id { get; set; }

        /// <summary>
        /// �û�����Id
        /// </summary>
        public String UserId { get; set; }

        /// <summary>
        /// Ȩ��
        /// </summary>
        public String PermissionValue { get; set; }

    }
}