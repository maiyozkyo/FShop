using FShop.Business.Base;
using FShop.Models.Request;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Newtonsoft.Json;
using FShop.Business.Fruit;
using Microsoft.AspNetCore.Authorization;
using FShop.Models.Fruit;

namespace FShop.Service.Fruit.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FruitController : ControllerBase
    {
        private static FruitBusiness fruitBusiess;
        private readonly IConfiguration _Configuration;
        public FruitController(IMongoDBContext dBContext, IConfiguration _IConfiguration)
        {
            _Configuration = _IConfiguration;
            fruitBusiess = new FruitBusiness(dBContext, _Configuration);
        }

        [HttpPost("test")]
        public async Task<IActionResult> Test()
        {
            return Ok("Test");
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddAsync([FromForm] FruitModel fruit)
        {
            var res = await fruitBusiess.AddUpdateFruit(fruit);
            return Ok(res);
        }

        [HttpPost("all")]
        public async Task<IActionResult> All([FromForm] RequestMsg requestMsg)
        {
            Type tType = fruitBusiess.GetType();
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

                var res = await (dynamic)method.Invoke(fruitBusiess, lstObj.ToArray());
                return Ok(res);
            }
            return NotFound();
        }
    }
}