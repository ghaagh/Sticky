using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky.Models.Etc
{

#pragma warning disable CA1027 // Mark enums with FlagsAttribute
    public enum RedisDatabases
#pragma warning restore CA1027 // Mark enums with FlagsAttribute
    {
        PartnerCookieMatch = 0,
        UserTotalVisits=1,
        CacheData =2,
        ActiveUsers = 3,
       UserSegmentsRequest=  4,
       BlockedCategory = 5,
        EventHubQueue=6,
        EmptyResult=7,
        Logs=8,
        Categories = 9,
        UserSegmentsZero = 10,
        ResultZero = 11,
        ResponseTiming=12,
        SegmentStats=13, TextTemplateV2 = 14

    }
    public enum ResultTypes
    {
        General = 0,
        SpecificForUser=1
    }
    public static class ResponseStatus
    {
        public const string Error = "Error";
        public const string Success = "True";
    }
    public static class MongoNames
    {
    public const string DBName = "TrackEm";
        public const string Host = "Host_";
        public const string Users = "_Users";
    }

    public static class CommonStrings
    {
        public const string Dot = ".";
        public const string NoHost = "NO HOST";
        public const string Origin = "Origin";
        public const string NoHostAccess = "NO ACCESS TO HOST";
    }
    public static class ResponseMessage
    {
        public const string InvalidData = "Invalid Data";
        public const string NotRegisteredPartner = "Not Registered Partner";
        public const string RequiredUserIdAndPartnerHashCode = "Your 'User Id' and 'Partner Hashcode' is required to get data.";
    }
    public static class RequestType
    {
        public const int Banner = 0;
        public const int Native = 1;
    }
    public static class StatTypes
    {
        public const string PageView = "PageView";
        public const string AddToCart = "AddToCart";
        public const string ProductView = "ProductView";
        public const string ProductPurchase = "ProductPurchase";
        public const string RemoveFromCart = "RemoveFromCart";
        public const string Like = "Like";
        public const string Unlike = "Unlike";
    }
}
