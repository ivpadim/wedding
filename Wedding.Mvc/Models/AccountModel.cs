using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using System.Web.Security;
using Microsoft.WindowsAzure.StorageClient;

namespace Wedding.Mvc.Models
{

    public class LoginModel
    {
        [Required]
        [Display(Name = "email")]
        public string Account { get; set; }

        [Required]
        [Display(Name = "password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "remember me?")]
        public bool RememberMe { get; set; }
    }

    public class UserData : TableServiceEntity
    {
        [Required(ErrorMessage = "email required")]
        [Display(Name = "email")]
        [RegularExpression(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Not a valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "password required")]
        [Display(Name = "password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "first name required")]
        [Display(Name = "first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "last name required")]
        [Display(Name = "last name")]
        public string LastName { get; set; }

        [Display(Name = "role")]
        public string Role { get; set; }

        public DateTime LastLogin { get; set; }

        public UserData()
            : base("wedding", (DateTime.MaxValue - DateTime.UtcNow).Ticks.ToString("d19") + Guid.NewGuid().ToString())
        {

        }

        public static UserData FromString(string stringData)
        {
            string[] properties = stringData.Split('|');
            return new UserData
            {
                Email = properties[0],
                FirstName = properties[1],
                LastName = properties[2],
                Role = properties[3],
                LastLogin = DateTime.Parse(properties[4])
            };
        }

        public override string ToString()
        {
            return string.Format("{0}|{1}|{2}|{3}|{4}",
                                                    this.Email,
                                                    this.FirstName,
                                                    this.LastName,
                                                    this.Role,
                                                    this.LastLogin.ToLongDateString());
        }
    }

    public class UserIdentity : IIdentity
    {

        public string AuthenticationType
        {
            get { return "AzureAuth"; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }

        public string Name
        {
            get { return Ticket.Name; }
        }

        public FormsAuthenticationTicket Ticket { get; set; }
    }

    public class UserPrincipal : IPrincipal
    {
        private UserIdentity _identity;
        public UserPrincipal(UserIdentity identity)
        {
            this._identity = identity;
        }

        public IIdentity Identity
        {
            get { return _identity; }
        }

        public bool IsInRole(string role)
        {
            return UserData.FromString(_identity.Ticket.UserData).Role == role;
        }
    }
}