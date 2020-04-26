using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CodeGenerator.Entity.Oms;

namespace CodeGenerator.Entity.Dto
{
    /// <summary>
    /// Oms_Order
    /// </summary>
    public class Oms_OrderDto:Oms_Order
    {

        /// <summary>
        /// 是否删除
        /// </summary>
        public string DeleteValue { get; set; }

    }
}