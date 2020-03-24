using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky.Models.Etc
{
    /// <summary>
    /// Contains All Changable Configuration from app.setting.
    /// </summary>
    public class Configurations
    {
        /// <summary>
        ///returns Sql conneciton String
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// returns Cookie Name for using in Cookie Setting.
        /// </summary>
        public string CookieName { get; set; }
        /// <summary>
        /// return Cookie domain for using in Cookie Setting.
        /// </summary>
        public string CookieDomain { get; set; }
        /// <summary>
        /// No Use Now.
        /// </summary>
        public int RedisDatabaseStartNumber { get; set; }
        public string FileAddressBase { get; set; }
        /// <summary>
        /// Address of Current Api in Internet.
        /// </summary>
        public string ApiAddress { get; set; }
        /// <summary>
        /// Indicates if we want to check the domain on fetch request or not.
        /// </summary>
        public bool SecurityCheck { get; set; }
        /// <summary>
        /// Indicates if we want to log data in kafka and then digest it in druid or not
        /// </summary>
        public bool DruidLogging { get; set; }
        /// <summary>
        /// Kafka Adress for posting streams.
        /// </summary>
        public string KafkaAddress { get; set; }
        public bool AzureLogging { get; set; }
    }
    public class MembershipRequestUpdaterConfiguration
    {
        public string MongoConnectionString { get; set; }
        public int EmptyExpirationInMinutes { get; set; }
        public string DruidClient { get; set; }
        public int? HostId { get; set; }
        public int? SegmentId { get; set; }
        public int BatchSize { get; set; }
        public bool Query { get; set; }
        public string StatType { get; set; }
        public bool Categories { get; set; }
        public string AerospikeClient { get; set; }
        public int FullExpirationInMinutes { get; set; }
    }
    public class DruidSegmentationConfiguration
    {
        public string DruidAPI { get; set; }
    }
    public class DashboardAPISetting
    {
        public string JWTSecret { get; set; }
        public string JwtIssuer { get; set; }
        public int JwtExpireDays { get; set; }
        public string ConnectionString { get; set; }
        public string ScriptLocation { get; set; }
        public int UserDataExpirationCache { get; set; }
        public string ScriptAPIUrlBase { get; set; }
    }
    public class AdvertisementAPISetting
    {
        public string AdvertisementUrlBase { get; set; }
    }

}
