using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Threading.Tasks;
using Blog.Core.AuthHelper;
using Blog.Core.AuthHelper.OverWrite;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CodeGenerator.BusinessService.IService;
using CodeGenerator.Entity.Crm;
using CodeGenerator.Entity.Dto;
using CodeGenerator.Entity.Enums;
using CodeGenerator.Util;

namespace CodeGenerator.BusinessService.Crm
{
    public class Crm_CustomerService : BaseService<Crm_Customer>, ICrm_CustomerService
    {
        static Crm_CustomerService _crm_CustomerService { get; } = new Crm_CustomerService();
        readonly PermissionRequirement _requirement;

        #region 外部接口

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="condition">查询类型</param>
        /// <param name="keyword">关键字</param>
        /// <returns></returns>
        public List<Crm_CustomerDto> GetDataList(string condition, string keyword, Pagination pagination)
        {
            var q = GetIQueryable();

            //模糊查询
            if (!condition.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
                q = q.Where($@"{condition}.Contains(@0)", keyword);
            var list = q.GetPagination(pagination).ToList().MapTo<Crm_CustomerDto>();
            list.ForEach(e =>
            {
                e.StatusValue = EnumExtension.GetEnumDescription(((EnumCustomerStatus)Enum.ToObject(typeof(EnumCustomerStatus), e.Status)));
                e.UserTypeValue = EnumExtension.GetEnumDescription(((EnumCustomerType)Enum.ToObject(typeof(EnumCustomerType), e.UserType)));
            });

            return list;
        }

        /// <summary>
        /// 获取指定的单条数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public Crm_CustomerDto GetTheData(string id)
        {
            var model = GetEntity(id).MapTo<Crm_CustomerDto>();

            model.UserTypeValue = EnumExtension.GetEnumDescription(((EnumCustomerType)Enum.ToObject(typeof(EnumCustomerType), model.UserType)));
            model.StatusValue = EnumExtension.GetEnumDescription(((EnumCustomerStatus)Enum.ToObject(typeof(EnumCustomerStatus), model.Status)));

            return model;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="newData">数据</param>
        public int AddData(Crm_CustomerDto newData)
        {
            newData.CustomerId = Guid.NewGuid().ToSequentialGuid();
            newData.CreateTime = DateTime.Now;
            newData.Password = newData.Password.Trim().ToMD5String();
            newData.No = NoExtension.ShortNo(Guid.NewGuid().GetHashCode().ToString());

            var result = Insert(newData);
            if (result == 0)
                throw new Exception("添加失败！");

            return result;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        public int UpdateData(Crm_CustomerDto theData)
        {
            var theUser = GetEntity(theData.CustomerId);
            if (theData.Password.Trim() != theUser.Password)
                theData.Password = theData.Password.Trim().ToMD5String();
            var result = Modify(theData, "Password", "UserType", "Status");

            if (result == 0)
                throw new Exception("更新失败！");

            return result;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="theData">删除的数据</param>
        public int DeleteData(List<string> ids)
        {
            var result = Delete(ids);
            if (result == 0)
                throw new Exception("删除失败！");

            return result;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="theData">登录信息</param>
        public Crm_CustomerDto CustomerLogin(Crm_CustomerDto dto)
        {
            dto.Password = dto.Password.ToMD5String();
            var result = GetIQueryable().Where(f => f.Name == dto.Name && f.Password == dto.Password).FirstOrDefault().MapTo<Crm_CustomerDto>();
            if (result.IsNullOrEmpty())
                throw new Exception("登录失败！");

            return result;
        }



        /// <summary>
        /// 获取用户类型名称
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Crm_CustomerDto GetUserTypeName(Crm_CustomerDto dto)
        {
            dto.Password = dto.Password.ToMD5String();
            var customer = _crm_CustomerService.GetIQueryable().Where(f => f.Name == dto.Name && f.Password == dto.Password).FirstOrDefault().MapTo<Crm_CustomerDto>();

            if (customer.IsNullOrEmpty())
                throw new Exception("登录失败！");

            customer.UserTypeValue = EnumExtension.GetEnumDescription(((EnumCustomerType)Enum.ToObject(typeof(EnumCustomerType), customer.UserType)));

            return customer;
        }


        /// <summary>
        /// 获取用户类型名称
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public dynamic GetJwtToken3(Crm_CustomerDto dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.Password))
                    throw new Exception("用户名或密码不能为空！");
                dto.Password = dto.Password.ToMD5String();
                var customer = _crm_CustomerService.GetIQueryable().Where(f => f.Name == dto.Name && f.Password == dto.Password).FirstOrDefault().MapTo<Crm_CustomerDto>();

                if (customer.IsNullOrEmpty())
                    throw new Exception("登录失败！");

                customer.UserTypeValue = EnumExtension.GetEnumDescription(((EnumCustomerType)Enum.ToObject(typeof(EnumCustomerType), customer.UserType)));


                //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
                var claims = new List<Claim> {
                        new Claim(ClaimTypes.Name, dto.Name),
                        new Claim(JwtRegisteredClaimNames.Jti, customer.CustomerId.ToString()),
                        new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_requirement.Expiration.TotalSeconds).ToString()) };
                claims.AddRange(customer.UserTypeValue.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));

                //用户标识
                var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
                identity.AddClaims(claims);

                var token = JwtToken.BuildJwtToken(claims.ToArray(), _requirement);
                return token;
            }
            catch (Exception ex)
            {
                throw new Exception("认证失败！");
            }
        }


        /// <summary>
        /// 获取用户类型名称
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public dynamic RefreshToken(Crm_CustomerDto dto)
        {
            try
            {
                string jwtStr = string.Empty;
                bool suc = false;

                if (string.IsNullOrEmpty(dto.Token))
                    throw new Exception("token无效，请重新登录！");
               
                var tokenModel = JwtHelper.SerializeJwt(dto.Token);
                if (tokenModel != null && !tokenModel.Uid.IsNullOrEmpty())
                    throw new Exception("token无效，请重新登录！");

                var customer = _crm_CustomerService.GetIQueryable().Where(f => f.Name == dto.Name && f.Password == dto.Password).FirstOrDefault().MapTo<Crm_CustomerDto>();

                if (customer.IsNullOrEmpty())
                    throw new Exception("登录失败！");

                customer.UserTypeValue = EnumExtension.GetEnumDescription(((EnumCustomerType)Enum.ToObject(typeof(EnumCustomerType), customer.UserType)));

                        //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
                        var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, customer.Name),
                    new Claim(JwtRegisteredClaimNames.Jti, tokenModel.Uid.ObjToString()),
                    new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_requirement.Expiration.TotalSeconds).ToString()) };
                        claims.AddRange(customer.UserTypeValue.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));

                        //用户标识
                        var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
                        identity.AddClaims(claims);

                        var refreshToken = JwtToken.BuildJwtToken(claims.ToArray(), _requirement);
                        return refreshToken;

            }
            catch (Exception)
            {
                throw new Exception("认证失败！");
            }
        }
        

        #endregion

        #region 私有成员

        #endregion

        #region 数据模型

        #endregion
    }
}