using FShop.Business.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FShop.Models.User
{
    public class UserDTO : BaseEntity
    {
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public bool Active { get; set; }
        public bool IsAdmin { get; set; }

        public static explicit operator UserDTO(UserModel v)
        {
            throw new NotImplementedException();
        }
    }
}
