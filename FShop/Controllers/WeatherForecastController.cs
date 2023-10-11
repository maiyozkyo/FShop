using FShop.Business.Base;
using FShop.Business.User;
using FShop.Models.User;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace FShop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static UserBusiness userBusiness;

        public WeatherForecastController(IMongoDBContext dbContext)
        {
            userBusiness = new UserBusiness(dbContext);
        }


        [HttpPost(Name ="test")]
        public IActionResult Test(UserModel user)
        {
            Type tType = userBusiness.GetType();
            MethodInfo method = tType.GetMethod("AddUserAsync");
            var res = method.Invoke(userBusiness, new object[] { user });
            return Ok(res);
        }
    }
}