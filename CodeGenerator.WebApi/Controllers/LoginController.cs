using Blog.Core.AuthHelper.OverWrite;
using Microsoft.AspNetCore.Mvc;
using CodeGenerator.BusinessService.IService;
using CodeGenerator.Entity.Dto;
using System;
using System.Threading.Tasks;

namespace CodeGenerator.WebApi.Controllers
{
    /// <summary>
    /// 登录
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : BaseController
    {
        private ICrm_CustomerService _crm_CustomerService { get; }
        public LoginController(ICrm_CustomerService crm_CustomerService)
        {
            _crm_CustomerService = crm_CustomerService;
        }


        #region 获取token的第1种方法
        /// <summary>
        /// 获取JWT的方法1
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="sub">角色</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Token")]
        public ActionResult GetJwtStr([FromBody] Crm_CustomerDto theData)
        {
            //这里就是用户登陆以后，通过数据库去调取数据，分配权限的操作
            var customer = _crm_CustomerService.GetUserTypeName(theData);
            if (customer != null)
            {
                var user = new UserEntities()
                {
                    userId = customer.CustomerId,
                    userName = customer.NickName,
                    userNo = customer.No,
                    loginTime = DateTime.Now
                };

                TokenModelJwt tokenModel = new TokenModelJwt { Uid = customer.CustomerId, Role = customer.UserTypeValue };

                user.token = JwtHelper.IssueJwt(tokenModel);
                return Success(user);
            }
            else
            {
                return Error("登录失败");
            }
        }


        /// <summary>
        /// 获取JWT的方法2：给Nuxt提供
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTokenNuxt")]
        public ActionResult GetJwtStrForNuxt(string name, string passWord)
        {
            string jwtStr = string.Empty;
            bool suc = false;
            //这里就是用户登陆以后，通过数据库去调取数据，分配权限的操作
            //这里直接写死了
            if (name == "admins" && passWord == "admins")
            {
                TokenModelJwt tokenModel = new TokenModelJwt();
                tokenModel.Uid = "admin";
                tokenModel.Role = "admin";

                jwtStr = JwtHelper.IssueJwt(tokenModel);
                suc = true;
            }
            else
            {
                jwtStr = "login fail!!!";
            }
            var result = new
            {
                data = new { success = suc, token = jwtStr }
            };

            return Ok(new
            {
                success = suc,
                data = new { success = suc, token = jwtStr }
            });
        }


        /// <summary>
        /// 获取JWT的方法3：整个系统主要方法
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("JWTToken3.0")]
        public JsonResult GetJwtToken3([FromBody] Crm_CustomerDto theData)
        {
            var token = _crm_CustomerService.GetJwtToken3(theData);
            return new JsonResult(token);
        }


        /// <summary>
        /// 请求刷新Token（以旧换新）
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RefreshToken")]
        public JsonResult RefreshToken([FromBody] Crm_CustomerDto theData)
        {
            var token = _crm_CustomerService.RefreshToken(theData);
            return new JsonResult(token);
        }


        #endregion

    }
}