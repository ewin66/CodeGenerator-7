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
        /// ��Ʒ���
        /// </summary>
        public String ProductNo { get; set; }

        /// <summary>
        /// ��Ʒ����
        /// </summary>
        public String ProductName { get; set; }

        /// <summary>
        /// �ɱ���
        /// </summary>
        public Decimal? CostPrice { get; set; }

        /// <summary>
        /// ��λ���
        /// </summary>
        public String Specification { get; set; }

        /// <summary>
        /// �Ƿ����
        /// </summary>
        public Boolean IsDisable { get; set; }

        /// <summary>
        /// �Ƿ����
        /// </summary>
        public string DisableValue { get; set; }

        /// <summary>
        /// ����״̬
        /// </summary>
        public string SaleStatusValue { get; set; }


    }
}