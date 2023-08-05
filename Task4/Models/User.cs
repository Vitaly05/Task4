using Microsoft.AspNetCore.Identity;
using Task4.Data;

namespace Task4.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }

        public DateTime RegisterDate { get; set; }

        public DateTime LastLogin { get; set; }

        public AccountStatus Status { get; set; } = AccountStatus.Active;
    }
}