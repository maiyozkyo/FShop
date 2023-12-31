﻿using FShop.Business.Base;
using FShop.Models.Fruit;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace FShop.Business.Fruit
{
    public class FruitBusiness : BaseBusiness<FruitModel>
    {
        private readonly IConfiguration Configuration;
        public FruitBusiness(IMongoDBContext dbContext, IConfiguration _IConfiguration) : base(dbContext)
        {
            Configuration = _IConfiguration;
        }

        public async Task<FruitModel> AddUpdateFruit(FruitModel fruit)
        {
            if (fruit == null)
            {
                return null;
            }
            
            try
            {
                var addedFruit = await Repository.GetOneAsync(f => f.RecID == fruit.RecID);
                //New fruit
                if (addedFruit != null)
                {

                }
                else
                {
                    fruit.CreatedOn = DateTime.UtcNow;
                    fruit.ModifiedOn = DateTime.UtcNow;
                    fruit.Active = true;
                    var res = await Repository.Add(fruit);
                    return res;
                }
            }
            catch (Exception ex)
            {

            }
            return fruit;
        }

        public async Task<List<FruitModel>> FakeFruits()
        {
            var fruits = FruitModel.Fake(20);
            await Repository.Add(fruits);
            return fruits;
        }

        public async Task<List<FruitModel>> GetListFruits(int page = 0)
        {
            var lstFruit = await Repository.GetPage(page).ToListAsync();
            return lstFruit;
        }
    }
}