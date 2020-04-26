using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;
using System.ComponentModel;

namespace CodeGenerator.Util
{

    /// <summary>
    /// Enum 扩展
    /// </summary>
    public class EnumExtension
    {

        /// <summary>
        /// 枚举字段描述列表
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static IList<string> GetEnumDescriptionList(Type t)
        {
            var valueDescList = Enum.GetValues(t).Cast<Enum>().Select(m => GetEnumDescription(m)).ToList();
            return valueDescList;
        }

        /// <summary>
        /// 枚举下拉列表
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static IList<SelectListItem> GetEnumList(Type t)
        {
            var enumList = Enum.GetValues(t).Cast<Enum>()
            .Select(m =>
            {
                return new SelectListItem()
                {
                    Text = GetEnumDescription(m),
                    Value = Enum.GetName(t, m)
                };
            }).ToList();
            return enumList;
        }

        /// <summary>
        /// 获取枚举的描述信息（Description特性）
        /// </summary>
        /// <param name="enumValue">枚举值</param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum enumValue)
        {
            Type type = enumValue.GetType();
            FieldInfo fi = type.GetField(enumValue.ToString());
            object[] attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (attrs.Length > 0)
                return ((DescriptionAttribute)attrs[0]).Description;
            return "";
        }

        /// <summary>
        /// 获取特性的描述
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetEnumAttributeDesc(Type type)
        {
            var attribute = type.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();
            if (attribute != null)
            {
                return ((DescriptionAttribute)attribute).Description;
            }
            return string.Empty;
        }


    }
}