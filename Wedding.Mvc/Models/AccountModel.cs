using System;
using Microsoft.WindowsAzure.StorageClient;
using System.ComponentModel.DataAnnotations;

namespace Wedding.Mvc.Models
{

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string Account { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class User : TableServiceEntity
    {
        public string Account { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string[] Roles { get; set; }

        public User()
            : base("users", (DateTime.MaxValue - DateTime.UtcNow).Ticks.ToString("d19") + Guid.NewGuid().ToString())
        {

        }
    }
}