using FShop.Business.Base;

namespace FShop.Models.User
{
    public class UserModel : BaseEntity
    {
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }    
        public string Token { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public bool Active { get; set; }
        public bool IsAdmin { get; set; }
        public string UserType { get; set; }
    }
}
