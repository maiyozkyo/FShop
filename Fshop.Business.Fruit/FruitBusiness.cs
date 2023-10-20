using FShop.Business.Base;
using FShop.Models.Fruit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fshop.Business.Fruit
{
    public class FruitBusiness : BaseBusiness<FruitModel>
    {
        public FruitBusiness(IMongoDBContext dbContext) : base(dbContext)
        {
        }


    }
}
