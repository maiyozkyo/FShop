using FShop.Business.Base;
using FShop.Models.User;
using MongoDB.Driver;

namespace FShop.Business.User
{
    public class UserBusiness : BaseBusiness<UserModel>
    {
        public UserBusiness(IMongoDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<UserModel> AddUpdateUserAsync(UserModel user)
        {
            if (user == null)
            {
                return null;
            }
            try
            {
                var addedUser = Repository.Get(x => x.Phone == user.Phone).FirstOrDefault();
                if (addedUser != null)
                {
                    addedUser.Phone = user.Phone;
                    addedUser.UserName = user.UserName;
                    addedUser.NickName = user.NickName;
                    addedUser.Email = user.Email;
                    //addedUser.Password = user.Password;
                    addedUser.ModifiedOn = DateTime.UtcNow;
                    await Repository.Update(addedUser.RecID, addedUser);
                    return addedUser;
                }
                else
                {
                    user.CreatedOn = DateTime.UtcNow;
                    user.ModifiedOn = DateTime.UtcNow;
                    user.Password = BCryptBusiness.Hash(user.Password);
                    var res = await Repository.Add(user);
                    return res;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<UserModel> GetUserByIDAsync(string id)
        {
            return await Repository.GetByID(id);
        }

        public async Task<UserModel> LoginAsync(string phone,string password) {
            var user = Repository.Get(x => x.Phone == phone && x.DeletedOn == null).FirstOrDefault();
            if (user != null)
            {
                if (BCryptBusiness.Verify(password, user.Password))
                {
                    //tao token
                }
                else
                {
                    user = null;
                }
            }
            return user;
        }

    }
}
