using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StickyNotesUsersAPIService.Models
{
    public class UserModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public bool IsAuthorised { get; set; }
    }
}
