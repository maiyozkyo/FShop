using FShop.Business.Base;
using FShop.Business.User;
using FShop.Models.User;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text.Json;

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
        public async Task<IActionResult> All()
        {
            var sMethod = Request.Form["Method"];
            var sParams = Request.Form["Params"];
            var param = sParams.ToDictionary(x => x);
            //var param = JsonSerializer.Deserialize<Dictionary<string, string>>(sParams);


            Type tType = userBusiness.GetType();
            MethodInfo method = tType.GetMethod(sMethod);
            if (method != null)
            {
                var paramInfos = method.GetParameters();
                var lstObj = new List<object>();
                for(var idx = 0; idx < paramInfos.Length; idx++)
                {
                    if (paramInfos[idx].ParameterType.isc)
                    var obj = 
                }

                foreach(var value in param.Values)
                {
                    lstObj.Add(value);
                }
                var res = await (dynamic)method.Invoke(userBusiness, lstObj.ToArray());
                return Ok(res);
            }
            return NotFound();
        }
    }
}