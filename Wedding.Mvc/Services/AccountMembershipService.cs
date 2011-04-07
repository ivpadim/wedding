﻿using System;
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
        int MinPasswordLength { get; }

        bool ValidateUser(string userName, string password);
        MembershipCreateStatus CreateUser(string userName, string password, string email);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
    }

    public class AccountMembershipService : IMembershipService
    {

        public AccountMembershipService()
        {
        }


        public int MinPasswordLength
        {
            get { return 6; }
        }

        public bool ValidateUser(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");

            var account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("DataConnectionString"));
            var context = account.CreateCloudTableClient().GetDataServiceContext();

            var adminEmail = RoleEnvironment.GetConfigurationSettingValue("AdminEmail");
            var adminPassword = RoleEnvironment.GetConfigurationSettingValue("AdminPassword");

            if (userName == adminEmail)
            {
                return adminEmail == userName && adminPassword == password;
            }
            else
            {
                var userAuth = context.CreateQuery<User>("Users")
                                        .Where(user => user.PartitionKey == "wedding" &&
                                                                     user.Email == userName &&
                                                                     user.Password == password)
                                        .FirstOrDefault();

                return userAuth != null;
            }
        }

        public void UpdateLastLogin(string userName)
        {
            var account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("DataConnectionString"));
            var context = account.CreateCloudTableClient().GetDataServiceContext();
            var user = context.CreateQuery<User>("Users")
                                    .Where(u => u.PartitionKey == "wedding" &&
                                                           u.Email == userName)
                                    .First();
            user.LastLogin = DateTime.Now;
            context.UpdateObject(user);
            context.SaveChangesWithRetries();
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string email)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");
            if (String.IsNullOrEmpty(email)) throw new ArgumentException("Value cannot be null or empty.", "email");

            MembershipCreateStatus status = MembershipCreateStatus.UserRejected;
            //_provider.CreateUser(userName, password, email, null, null, true, null, out status);
            return status;
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(oldPassword)) throw new ArgumentException("Value cannot be null or empty.", "oldPassword");
            if (String.IsNullOrEmpty(newPassword)) throw new ArgumentException("Value cannot be null or empty.", "newPassword");

            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
            try
            {
                //MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
                //return currentUser.ChangePassword(oldPassword, newPassword);
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (MembershipPasswordException)
            {
                return false;
            }
        }
    }

}