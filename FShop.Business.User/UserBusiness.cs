using FShop.Business.Base;
using FShop.Models.User;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace FShop.Business.User
{
    public class UserBusiness : BaseBusiness<UserModel>
    {
        private readonly IConfiguration Configuration;
        private readonly string jwtSerect;
        public UserBusiness(IMongoDBContext dbContext, IConfiguration _IConfiguration) : base(dbContext)
        {
            Configuration = _IConfiguration;
            jwtSerect = Configuration.GetSection("JWTSerect").Value;
        }

        public async Task<UserModel> AddUpdateUserAsync(UserModel user)
        {
            if (user == null)
            {
                return null;
            }
            try
            {
                var addedUser = await Repository.GetOneAsync(x => x.Phone == user.Phone);
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

        public async Task<UserDTO> LoginAsync(string phone,string password) {
            var user = await Repository.GetOneAsync(x => x.Phone == phone && x.DeletedOn == null);
            if (user != null)
            {
                if (BCryptBusiness.Verify(password, user.Password))
                {
                    //tao token
                    user = await CreateJWTTokenAsync(user);
                    var userDTO = JsonSerializer.Deserialize<UserDTO>(JsonSerializer.Serialize(user));
                    userDTO.ReToken = GenReToken();
                    return userDTO;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        private async Task<UserModel> CreateJWTTokenAsync(UserModel user)
        {
            if (user != null)
            {
                try
                {
                    var jwtTokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(jwtSerect);

                    var identity = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, $"{user.Phone}"),
                    new Claim(ClaimTypes.Role, user.Role ?? "")
                    });

                    var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512);

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = identity,
                        Expires = DateTime.UtcNow.AddDays(1),
                        SigningCredentials = credentials,
                    };

                    var token = jwtTokenHandler.CreateToken(tokenDescriptor);
                    user.Token = jwtTokenHandler.WriteToken(token);
                }
                catch (Exception ex)
                {
                    return null;
                }
                
            }
            return user;
        }

        private string GenReToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
