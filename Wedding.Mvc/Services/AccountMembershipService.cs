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
        MembershipCreateStatus CreateUser(UserData userData);
        MembershipCreateStatus EditUser(UserData userData);
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

        public MembershipCreateStatus CreateUser(UserData userInfo)
        {
            if (String.IsNullOrEmpty(userInfo.Email)) throw new ArgumentException("Value cannot be null or empty.", "Email");
            if (String.IsNullOrEmpty(userInfo.Password)) throw new ArgumentException("Value cannot be null or empty.", "Password");
            if (String.IsNullOrEmpty(userInfo.FirstName)) throw new ArgumentException("Value cannot be null or empty.", "First Name");
            if (String.IsNullOrEmpty(userInfo.LastName)) throw new ArgumentException("Value cannot be null or empty.", "First Name");

            if (String.IsNullOrEmpty(userInfo.Role))
                userInfo.Role = "Guest";



            var account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("DataConnectionString"));
            var context = account.CreateCloudTableClient().GetDataServiceContext();

            var currentUser = context.CreateQuery<UserData>("Users")
                        .Where(user => user.PartitionKey == "wedding" &&
                                                     user.Email == userInfo.Email)
                        .FirstOrDefault();

            if (currentUser != null)
                return MembershipCreateStatus.DuplicateUserName;

            context.AddObject("Users", userInfo);
            context.SaveChangesWithRetries();

            return MembershipCreateStatus.Success;
        }

        public MembershipCreateStatus EditUser(UserData userInfo)
        {
            if (String.IsNullOrEmpty(userInfo.Password)) throw new ArgumentException("Value cannot be null or empty.", "Password");
            if (String.IsNullOrEmpty(userInfo.FirstName)) throw new ArgumentException("Value cannot be null or empty.", "First Name");
            if (String.IsNullOrEmpty(userInfo.LastName)) throw new ArgumentException("Value cannot be null or empty.", "First Name");


            var account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("DataConnectionString"));
            var context = account.CreateCloudTableClient().GetDataServiceContext();

            var currentUser = context.CreateQuery<UserData>("Users")
                        .Where(user => user.PartitionKey == "wedding" &&
                                                     user.Email == userInfo.Email)
                        .FirstOrDefault();

            if (currentUser == null)
                return MembershipCreateStatus.InvalidUserName;

            currentUser.FirstName = userInfo.FirstName;
            currentUser.LastName = userInfo.LastName;
            currentUser.Password = userInfo.Password;

            context.UpdateObject(currentUser);
            context.SaveChangesWithRetries();

            return MembershipCreateStatus.Success;
        }

    }

    public static class AccountValidation
    {
        public static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Username already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A username for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
    }

}