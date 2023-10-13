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
            var form = Request.Form.Where(x => x.Key != "Method").ToDictionary(x => x.Key, y => y.Value.ToString());
            Type tType = userBusiness.GetType();
            MethodInfo method = tType.GetMethod(sMethod);
            if (method != null)
            {
                var res = await (dynamic)method.Invoke(userBusiness, form.Values.Cast<object>().ToArray());
                return Ok(res);
            }
            return null;
        }
    }
}