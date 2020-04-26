using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CodeGenerator.Entity.Oms;

namespace CodeGenerator.Entity.Dto
{
    /// <summary>
    /// Oms_Address
    /// </summary>
    public class Oms_AddressDto:Oms_Address
    {

        /// <summary>
        /// 是否默认
        /// </summary>
        public string DefaultValue { get; set; }

    }
}