using System.Collections;
using System.Collections.Generic;
using AutoMapper;
using System;
using System.Reflection;
using System.Data;

namespace CodeGenerator.Util
{
    /// <summary>
    /// automapper 扩展
    /// </summary>
    public static class MapperExtension
    {
        /// <summary>
        /// 对象对对象
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TDestination MapTo<TDestination>(this object source)
        {
            if (source == null) return default(TDestination);
            //请确定是否在DtoMapper.cs 中配置了模型映射关系
            return Mapper.Map<TDestination>(source);
        }
        /// <summary>
        /// 集合对集合
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<TDestination> MapTo<TDestination>(this IEnumerable source)
        {
            //请确定是否在DtoMapper.cs 中配置了模型映射关系
            return Mapper.Map<List<TDestination>>(source);
        }
    }
}
