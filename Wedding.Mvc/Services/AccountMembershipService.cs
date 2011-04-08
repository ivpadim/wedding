using System;
using System.Linq;
using System.Web.Security;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using Wedding.Mvc.Models;

namespace Wedding.Mvc.Services
{
    public interface IMembershipService
    {
        bool AuthenticateUser(string userName, string password);        
    }

    public class AccountMembershipService : IMembershipService
    {

        public UserData User { get; set; }

        public bool AuthenticateUser(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");

            var account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("DataConnectionString"));
            var context = account.CreateCloudTableClient().GetDataServiceContext();

            var userAuth = context.CreateQuery<UserData>("Users")
                        .Where(user => user.PartitionKey == "wedding" &&
                                                     user.Email == userName &&
                                                     user.Password == password)
                        .SingleOrDefault();

            if (userAuth != null)
            {
                userAuth.LastLogin = DateTime.Now;
                context.UpdateObject(userAuth);
                context.SaveChangesWithRetries();

                User = userAuth;
                return true;
            }
            else
            {
                return false;
            }
        }      
    }

}