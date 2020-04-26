using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CodeGenerator.Entity.Scm;

namespace CodeGenerator.Entity.Dto
{
    /// <summary>
    /// Scm_Product
    /// </summary>
    public class Scm_ProductDto:Scm_Product
    {
        /// <summary>
        /// �Ƿ����
        /// </summary>
        public string DisableValue { get; set; }
        /// <summary>
        /// �ؼ���
        /// </summary>
        public string Keyword { get; set; }

        

    }
}