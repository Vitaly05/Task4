using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Task4.Data;
using Task4.Models;

namespace Task4.Repositories.Implementations
{
    public class UsersRepository : IUsersRepository
    {
        private UserManager<User> userManager;

        public UsersRepository(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<User> GetUserAsync(ClaimsPrincipal principal) => await userManager.GetUserAsync(principal);

        public async Task<List<User>> GetAllAsync() => await userManager.Users.ToListAsync();

        public async Task<IdentityResult> CreateAsync(User user, string password) => await userManager.CreateAsync(user, password);

        public async Task<IdentityResult> DeleteAsync(User user) => await userManager.DeleteAsync(user);

        public IEnumerable<User> GetUsers(IEnumerable<string> usersIds)
        {
            if (usersIds is not null)
                foreach (var id in usersIds)
                    yield return userManager.Users.FirstOrDefault(u => u.Id == id);
        }

        public async Task ChangeStatus(User user, AccountStatus status)
        {
            if (user is null) return;
            user.Status = status;
            await userManager.UpdateAsync(user);
        }
    }
}