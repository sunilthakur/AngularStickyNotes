using Microsoft.EntityFrameworkCore;
using StickyNotesUsersAPIService.Data;
using StickyNotesUsersAPIService.Entities;
using StickyNotesUsersAPIService.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StickyNotesUsersAPIService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext context;
        public UserRepository(UserDbContext userContext)
        {
            context = userContext ?? throw new ArgumentNullException(nameof(userContext));
        }
        public async Task<Users> Create(Users user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return await GetUserById(user.UserId);
        }

        public async Task<Users> GetUserById(int userId)
        {
            var user = context.Users.FindAsync(userId);
            return await user;
        }

        public async Task<Users> ValidateUserLogin(string userName,string password)
        {
            var user = context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == userName.ToLower() && x.Password.ToLower() == password.ToLower());
            if (user != null)
            {
                return await user;
            }
            else
            {
                return null;
            }
        }
    }
}
