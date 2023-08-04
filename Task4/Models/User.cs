using System.ComponentModel.DataAnnotations;
using Task4.Data;

namespace Task4.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Name { get; set; } = "";

        public string Email { get; set; } = "";

        public DateTime RegisterDate { get; set; }

        public DateTime LastLogin { get; set; }

        public AccountStatus Status { get; set; } = AccountStatus.Active;
    }
}
