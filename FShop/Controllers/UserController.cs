using FShop.Business.Base;
using FShop.Business.User;
using FShop.Models.Request;
using FShop.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;

namespace FShop.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private static UserBusiness userBusiness;
        private readonly IConfiguration _Configuration;
        public UserController(IMongoDBContext dbContext, IConfiguration _IConfiguration)
        {
            _Configuration = _IConfiguration;
            userBusiness = new UserBusiness(dbContext, _Configuration);
        }

        [HttpPost("test")]
        public async Task<IActionResult> Test()
        {
            return Ok("OK");
        }

        [AllowAnonymous]
        [HttpPost("LoginAsync")]
        public async Task<IActionResult> LoginAsync([FromForm] RequestMsg requestMsg)
        {
            if (!string.IsNullOrEmpty(requestMsg.Params))
            {
                var lstParams = JsonConvert.DeserializeObject<List<string>>(requestMsg.Params);
                var phone = JsonConvert.DeserializeObject<string>(lstParams[0]);
                var pw = JsonConvert.DeserializeObject<string>(lstParams[1]);
                var loginUser = await userBusiness.LoginAsync(phone, pw);
                if (loginUser != null)
                {
                    return Ok(loginUser);
                }
                else
                {
                    return NotFound("Không tìm thấy user");
                }
            }
            return BadRequest("Sai");
        }

        [HttpPost("all")]
        public async Task<IActionResult> All([FromForm] RequestMsg requestMsg)
        {
            Type tType = userBusiness.GetType();
            MethodInfo method = tType.GetMethod(requestMsg.Method);
            if (method != null)
            {
                var lstObj = new List<object>();
                var lstParams = JsonConvert.DeserializeObject<List<string>>(requestMsg.Params);

                var paramInfos = method.GetParameters();
                for (var idx = 0; idx < paramInfos.Length; idx++)
                {
                    var type = paramInfos[idx].ParameterType;
                    var obj = JsonConvert.DeserializeObject(lstParams[idx], type);
                    lstObj.Add(obj);
                }

                var res = await (dynamic)method.Invoke(userBusiness, lstObj.ToArray());
                return Ok(res);
            }
            return NotFound();
        }
    }
}