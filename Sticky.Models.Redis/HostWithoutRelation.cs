using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Models.Redis
{
    public class HostWithoutRelation
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Host { get; set; }
        public string HashCode { get; set; }
        public bool HostValidated { get; set; }
        public bool PageValidated { get; set; }
        public bool ProductValidated { get; set; }
        public bool CategoryValidated { get; set; }
        public bool AddToCardValidated { get; set; }
        public bool FinalizeValidated { get; set; }
        public string ValidatingHtmlAddress { get; set; }
        public string AddToCardId { get; set; }
        public string FinalizePage { get; set; }
        public int? UserValidityId { get; set; }
        public int? ProductValidityId { get; set; }
        public bool? HostPriority { get; set; }
        public string LogoAddress { get; set; }
        public int? ProductImageWidth { get; set; }
        public int? ProductImageHeight { get; set; }
        public string LogoOtherData { get; set; }
    }
}
