﻿using Microsoft.AspNetCore.Http;
using CodeGenerator.Util;

namespace CodeGenerator.BusinessService.IService
{
    public interface ICheckSignService
    {
        /// <summary>
        /// 判断是否有权限操作接口
        /// </summary>
        /// <returns></returns>
        bool IsSecurity(HttpContext context);

        /// <summary>
        /// 获取应用密钥
        /// </summary>
        /// <param name="appId">应用Id</param>
        /// <returns></returns>
        string GetAppSecret(string appId);

      
    }
}