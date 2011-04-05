using System;
using Microsoft.WindowsAzure.StorageClient;

namespace Wedding.Mvc.Models
{
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