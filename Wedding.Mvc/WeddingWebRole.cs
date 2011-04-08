using System;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using Wedding.Mvc.Models;

namespace Wedding.Mvc
{
    public class WeddingWebRole : RoleEntryPoint
    {
        public override bool OnStart()
        {
            var account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("DataConnectionString"));

            var wishesCreated = account.CreateCloudTableClient().CreateTableIfNotExist("Wishes");
            var usersCreated = account.CreateCloudTableClient().CreateTableIfNotExist("Users");
            var context = account.CreateCloudTableClient().GetDataServiceContext();
            
            if (usersCreated)
            {
                var admin = new UserData
                {
                    Email = RoleEnvironment.GetConfigurationSettingValue("AdminEmail"),
                    FirstName = RoleEnvironment.GetConfigurationSettingValue("AdminFirstName"),
                    LastName = RoleEnvironment.GetConfigurationSettingValue("AdminLastName"),
                    Password = RoleEnvironment.GetConfigurationSettingValue("AdminPassword"),
                    LastLogin = new DateTime(1900,1,1),
                    Role = "Administrator"
                };
                context.AddObject("Users", admin);
                context.SaveChangesWithRetries();
            }
            return base.OnStart();
        }
    }
}