using System;
using Microsoft.WindowsAzure.StorageClient;

namespace Wedding.Mvc.Models
{
    public class SongRequest : TableServiceEntity
    {
        public string Account { get; set; }
        public string From { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }

        public SongRequest()
            : base("wedding", (DateTime.MaxValue - DateTime.UtcNow).Ticks.ToString("d19") + Guid.NewGuid().ToString())
        {

        }
    }
}