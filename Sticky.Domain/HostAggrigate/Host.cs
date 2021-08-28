using System;
using System.Collections.Generic;
using System.Linq;
using Sticky.Domain.HostAggrigate.Exceptions;
using Sticky.Domain.SegmentAggrigate;

namespace Sticky.Domain.HostAggrigate
{
    public class Host : BaseEntity, IAggrigateRoot
    {
        private Host()
        {

        }

        public Host(string hostAddress, string userId, ValidityEnum userExpiration = ValidityEnum.Y1, ValidityEnum productExpiration = ValidityEnum.Y1) : base()
        {
            var guid= Guid.NewGuid().ToString();
            HostAddress = hostAddress;
            HashCode = guid;
            CreateDate = DateTime.Now;
            LastUpdateDate = DateTime.Now;
            IsDeleted = false;
            ProductExpiration = productExpiration;
            UserExpiration = userExpiration;
            _userAccesses.Add(new UsersHostAccess(userId, true));
        }

        private readonly HashSet<Segment> _segments = new HashSet<Segment>();
        private readonly HashSet<PagePattern> _pagePatterns = new HashSet<PagePattern>();
        private HashSet<UsersHostAccess> _userAccesses = new HashSet<UsersHostAccess>();
        public string HostAddress { get; private set; }
        public string HashCode { get; private set; }
        public bool HostValidated { get; private set; }
        public bool PageValidated { get; private set; }
        public bool ProductValidated { get; private set; }
        public bool CategoryValidated { get; private set; }
        public bool? FavValidated { get; private set; }
        public bool AddToCardValidated { get; private set; }
        public bool BuyValidated { get; private set; }
        public ValidityEnum UserExpiration { get; private set; }
        public ValidityEnum ProductExpiration { get; private set; }

        public IReadOnlyCollection<Segment> Segments => _segments;
        public IReadOnlyCollection<PagePattern> PagePatterns => _pagePatterns;
        public ICollection<UsersHostAccess> UserAccesses {
            set
            {
                _userAccesses = (HashSet<UsersHostAccess>)value;
            }
            get
            {
                return _userAccesses;
            }
        }


        public void ValidateHost() => HostValidated = true;

        public void ValidatePage() => PageValidated = true;

        public void ValidateProduct() => ProductValidated = true;

        public void ValidateCategory() => CategoryValidated = true;

        public void ValidateFav() => FavValidated = true;

        public void ValidateBuy() => BuyValidated = true;

        public void ValidateCart() => AddToCardValidated = true;

        //public void AddSegment(string name, ActivityTypeEnum activityType, ActionTypeEnum actionType, string actionExtra = "", bool isPublic = false)
        //{
        //    var segment = new Segment(name, activityType, actionType, actionExtra, isPublic);
        //    _segments.Add(segment);
        //}

        //public void AddPattern(string name, string pattern)
        //{
        //    var patternObj = new PagePattern(name, pattern);
        //    _pagePatterns.Add(patternObj);
        //}

        public void AccessToUser(string userId, bool isAdmin)
        {
            var patternObj = new UsersHostAccess(userId, isAdmin);
            _userAccesses.Add(patternObj);
        }
        public void RemoveAccess(string userId)
        {
            var currentAccess = _userAccesses.FirstOrDefault(c => c.UserId == userId);
            if (currentAccess == null)
                throw new UserAccessNotFoundException();
            _userAccesses.Remove(currentAccess);
        }
        public void AddSegment(Segment segment)
        {
            _segments.Add(segment);
        }
        public void RemoveSegment(long segmentId)
        {
            _segments.Remove(_segments.FirstOrDefault(c => c.Id == segmentId));
        }

        public bool IsAssinedTo(string userId) => UserAccesses.Any(c => c.UserId == userId);

        public bool IsOwnedBy(string userId)=> UserAccesses.Any(c => c.UserId == userId && c.AdminAccess);



    }
}
