using FShop.Business.Base;
using FShop.Business.User;
using FShop.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;

namespace FShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static UserBusiness userBusiness;

        public WeatherForecastController(IMongoDBContext dbContext)
        {
            userBusiness = new UserBusiness(dbContext);
        }

        [HttpPost("test")]
        public async Task<IActionResult> Test()
        {
            return Ok("OK");
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