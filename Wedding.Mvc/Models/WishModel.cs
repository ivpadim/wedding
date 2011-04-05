using System;
using Microsoft.WindowsAzure.StorageClient;

namespace Wedding.Mvc.Models
{

    public class Wish : TableServiceEntity
    {
        public string Account { get; set; }
        public string From { get; set; }
        public string Message { get; set; }
        public DateTime PublishDate { get; set; }

        public Wish()
            : base("wishes", (DateTime.MaxValue - DateTime.UtcNow).Ticks.ToString("d19") + Guid.NewGuid().ToString())
        {

        }
    }
}