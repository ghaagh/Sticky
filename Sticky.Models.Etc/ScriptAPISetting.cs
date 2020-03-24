using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Models.Etc
{
    public class ScriptAPISetting
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
        /// Indicates if we want to check the domain on fetch request or not.
        /// </summary>
        public bool SecurityCheck { get; set; }
        /// <summary>
        /// Kafka Adress for posting streams.
        /// </summary>
        public string KafkaAddress { get; set; }
    }
}
