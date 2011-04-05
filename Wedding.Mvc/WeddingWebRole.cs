using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace Wedding.Mvc
{
    public class WeddingWebRole : RoleEntryPoint
    {
        public override bool OnStart()
        {
            var account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("DataConnectionString"));
            account.CreateCloudTableClient().CreateTableIfNotExist("Wishes");
            account.CreateCloudTableClient().CreateTableIfNotExist("Users");
            return base.OnStart();
        }
    }
}