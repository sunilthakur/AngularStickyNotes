using StickyNotesUsersAPIService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StickyNotesUsersAPIService.Repositories.Interfaces
{
    public interface IUserRepository
    {
        //Task<IEnumerable<Users>> GetUsers();
        Task<Users> GetUserById(int userId);
        Task<Users> Create(Users user);
        Task<Users> ValidateUserLogin(string userName, string password);
        //Task<bool> Update(Users user);
        //Task<bool> Delete(string id);
    }
}
