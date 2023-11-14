using Bogus;
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
        public string Description { get; set; }
        public bool Active { get; set; }
        public string Image { get; set; }

        public static List<FruitModel> Fake(int quantity)
        {
            var faker = new Faker<FruitModel>()
                .RuleFor(o => o.FruitName, f => f.Commerce.ProductName())
                .RuleFor(o => o.Price, f => double.Parse(f.Commerce.Price()))
                .RuleFor(o => o.Unit, f => f.Commerce.ProductMaterial())
                .RuleFor(o => o.Image, f => f.Image.Food(640, 480, false, true))
                .RuleFor(o => o.Description, f => f.Commerce.ProductDescription())
                .RuleFor(o => o.Active, f => true)
                ;
            return faker.Generate(quantity);
        }
    }
}
