﻿namespace Sticky.Infrastructure.Message
{
    public class MessageModel
    {
        public string Date { get; set; }
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public int? Price { get; set; }
        public string PageAddress { get; set; }
        public string ImageAddress { get; set; }
        public string HostId { get; set; }
        public string HostName { get; set; }
        public string StatType { get; set; }
    }
}
