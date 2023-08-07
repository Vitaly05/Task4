using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Task4.Data;
using Task4.Models;

namespace Task4.Repositories
{
    public interface IUsersRepository
    {
        Task<User> GetUserAsync(ClaimsPrincipal principal);

        Task<List<User>> GetAllAsync();

        IEnumerable<User> GetUsers(IEnumerable<string> usersIds);

        Task<IdentityResult> CreateAsync(User user, string password);

        Task<IdentityResult> DeleteAsync(User user);

        Task<AccountStatus> GetStatusAsync(string email);

        Task ChangeStatus(User user, AccountStatus status);
    }
}