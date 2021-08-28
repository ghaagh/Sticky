using Newtonsoft.Json;
using System;

namespace Sticky.Infrastructure.Cache.Models
{

    public class ProductCache : CacheModel
    {
        public string Id { get; set; }
        public string HostName { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }
        public string ImageAddress { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsAvailable { get; set; }
        public string CategoryName { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public string Key
        {
            get{
                return $"{HostName}_{Id}";
            }
        }
    }
}
