using System.ComponentModel;

namespace CodeGenerator.Entity.Enums
{
       
        /// <summary>
        /// 是否
        /// </summary>
        public enum EnumWhether
        {
            [Description("否")]
            False = 0,
            [Description("是")]
            True = 1
        }
}
