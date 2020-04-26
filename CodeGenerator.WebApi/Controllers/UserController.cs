using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CodeGenerator.BusinessService.IService;
using CodeGenerator.Entity.Dto;

namespace CodeGenerator.WebApi.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private ICrm_CustomerService _crm_CustomerService { get; }
        public UserController(ICrm_CustomerService crm_CustomerService)
        {
            _crm_CustomerService = crm_CustomerService;
        }

        // POST api/user/userlogin
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="theData"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UserLogin([FromForm] Crm_CustomerDto theData)
        {
            theData = _crm_CustomerService.CustomerLogin(theData);

            return Ok((Crm_CustomerDto)theData);
        }


    }
}