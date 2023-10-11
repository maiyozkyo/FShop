using FShop.Business.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FShop.Models.User
{
    public class UserModel : BaseEntity
    {
        public string UserName { get; set; }
        public string Password { get; set; }    
    }
}
