using Sticky.Domain.UserAggrigate;
using System;
using System.Collections.Generic;

namespace Sticky.Domain.PartnerAggrigate
{
    public class Partner : BaseEntity,IAggrigateRoot
    {
        private Partner() { }
        public Partner(string userId,string name, string domain,string cookieAddress = "")
        {
            Name = name;
            Hash = Guid.NewGuid().ToString();
            CookieSyncAddress = cookieAddress;
            Domain = domain;
            UserId = userId;
            Verified = true;
        }
        public string Name { get; private set; }
        public string Hash { get; private set; }
        public string Domain { get; private set; }
        public string CookieSyncAddress { get; private set; }
        public bool Verified { get; private set; }
        public string UserId { get; private set; }
        public IIdentity User { get; private set; }

        public void Verify() => Verified = true;
        public void Unverify() => Verified = false;

        public Partner SetCookieAddress(string cookieMatchAddress)
        {
            if(!string.IsNullOrEmpty(cookieMatchAddress))
                CookieSyncAddress = cookieMatchAddress;
            return this;
        }
        public Partner SetName(string name)
        {
            if (!string.IsNullOrEmpty(name))
                Name = name;
            return this;
        }

        public Partner SetDomain(string domain)
        {
            if (!string.IsNullOrEmpty(domain))
                Domain = domain ;
            return this;
        }

    }
}
