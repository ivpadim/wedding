using System;
using Microsoft.WindowsAzure.StorageClient;

namespace Wedding.Mvc.Models
{
    public class News : TableServiceEntity
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string PublishDate { get; set; }

        public News()
            : base("wedding", (DateTime.MaxValue - DateTime.UtcNow).Ticks.ToString("d19") + Guid.NewGuid().ToString())
        {

        }
    }
}