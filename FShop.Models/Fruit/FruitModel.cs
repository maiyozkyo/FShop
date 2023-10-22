using FShop.Business.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FShop.Models.Fruit
{
    public class FruitModel : BaseEntity
    {
        public string FruitName { get; set; }
        public double Price { get; set; }
        public string Unit { get; set; }
        public string Note { get; set; }
        public bool Active { get; set; }
    }
}
