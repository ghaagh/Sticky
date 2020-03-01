using Microsoft.EntityFrameworkCore;
using Sticky.Dto.Dashboard.Request;
using Sticky.Dto.Dashboard.Response;
using Sticky.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky.Repositories.Dashboard.Implementions
{
    public class SegmentManager : ISegmentManager
    {
        private readonly StickyDbContext _db;
        private readonly IUserSearchManager _userSearchManager;
        public SegmentManager(StickyDbContext db, IUserSearchManager userSearchManager)
        {
            _userSearchManager = userSearchManager;
            _db = db;
        }
        public async Task<Segment> CreateSegmentAsync(CreateDruidSegmentRequest request)
        {
            using (var dbtr = _db.Database.BeginTransaction())
            {

                try
                {

                    var userId = await _userSearchManager.SearchUser(request.Email);
                    var host = _db.Hosts.FirstOrDefaultAsync(c => c.UserId == userId && request.HostId == c.Id);
                    if (host == null)
                        return new Segment();
                    var newSegment = new Segment()
                    {
                        CreateDate = DateTime.Now,
                        CreatorId = userId,
                        HostId = request.HostId,
                        Paused = false,
                        IsPublic = false,
                        ActionId = request.ActionId,
                        ActionExtra = request.ActionExtra,
                        AudienceId = request.AudienceId,
                        AudienceExtra = request.AudienceExtra,
                        SegmentName = request.SegmentName,

                    };
                    await _db.Segments.AddAsync(newSegment);
                    await _db.SaveChangesAsync();
                    dbtr.Commit();
                    return newSegment;
                }
                catch
                {
                    dbtr.Rollback();
                    return new Segment();
                }
            }



        }
        public async Task<bool> DeleteSegmentAsync(int segmentId)
        {
            var segment = await _db.Segments.FirstOrDefaultAsync(c => c.Id == segmentId);
            segment.IsDeleted = true;
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<SegmentResult> GetByIdAsync(int segmentId)
        {
            var row = await _db.Segments.Where(v=>v.Id==segmentId).Include(c => c.Creator).Include(c => c.Host).Where(c=>c.IsDeleted==false).Select(c => new SegmentResult { IsPublic=c.IsPublic,IsPaused = c.Paused, CreateDate = c.CreateDate,Id = c.Id, Name = c.SegmentName, Owner = c.Creator.UserName, Host = new HostData() { Address = c.Host.HostAddress, Id = c.HostId } }).FirstOrDefaultAsync();
            return row;
        }
        public async Task<bool> PublicAccessToggleAsync(string email,int segmentId)
        {
            var userId = await _userSearchManager.SearchUser(email);
            var segment =await _db.Segments.FirstOrDefaultAsync(c => c.Id == segmentId);
            if (segment.CreatorId != userId)
                return false;
            if (segment.ActionId !=1)
                return false;
            if (segment.IsPublic == null)
                segment.IsPublic = true;
            else
            segment.IsPublic = !segment.IsPublic;
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<List<SegmentResult>> GetUserSegmentsAsync(string email,int hostId)
        {
            var userId = await _userSearchManager.SearchUser(email);
            var row = await _db.Segments.Where(c => (c.HostId == hostId | hostId==0)).Include(c=>c.Creator).Include(c=>c.Host).Select(c => new SegmentResult {UserCount=c.AudienceNumber, AudieceExtra=c.AudienceExtra,ActionExtra=c.ActionExtra,IsPublic = c.IsPublic,ActionId=c.ActionId,IsPaused=c.Paused,CreateDate=c.CreateDate,AudienceId=c.AudienceId,Id = c.Id, Name = c.SegmentName,Owner=c.Creator.UserName,Host=new HostData() {Address=c.Host.HostAddress,Id=c.HostId } }).ToListAsync();
            return row;
        }
        public async Task<List<SegmentResult>> GetPublicSegmentsAsync(string email)
        {
            var userId = await _userSearchManager.SearchUser(email);
            var row = await _db.Segments.Include(c=>c.Host).Where(c => (c.IsPublic==true || c.Host.UserId==userId)).Include(c => c.Creator).Select(c => new SegmentResult {UserCount=c.AudienceNumber, IsPublic = c.IsPublic, IsPaused = c.Paused, CreateDate = c.CreateDate, Id = c.Id, Name = c.SegmentName, Owner = c.Creator.UserName, Host = new HostData() { Address = c.Host.HostAddress, Id = c.HostId } }).ToListAsync();
            return row;
        }

        public async Task<bool> PlayPauseSegmentAsync(int segmentId)
        {
            try
            {
            var row = await _db.Segments.FirstOrDefaultAsync(c => c.Id == segmentId);
            row.Paused = !row.Paused;
           await _db.SaveChangesAsync();
            return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task<bool> UpdateNativesAsync(SegmentNativeRequest request)
        {
            var userId = await _userSearchManager.SearchUser(request.Email);
            var segment = _db.Segments.Where(c => c.CreatorId == userId && c.Id == request.SegmentId);
            if (await segment.AnyAsync())
                return false;
            var currentRow = await _db.SegmentStaticNatives.FirstOrDefaultAsync(c => c.SegmentId == request.SegmentId);
            if (currentRow == null)
            {
                var newRow = new SegmentStaticNative() { SegmentId = request.SegmentId, NativeLogoAddress = request.NativeLogoAddress, NativeLogoOtherData = request.NativeLogoOtherData, NativeText = request.NativeText };
                await _db.SegmentStaticNatives.AddAsync(newRow);
                await _db.SaveChangesAsync();
            }
            else
            {
                currentRow.NativeLogoAddress = request.NativeLogoAddress;
                currentRow.NativeLogoOtherData = request.NativeLogoOtherData;
                currentRow.NativeText = request.NativeText;
                await _db.SaveChangesAsync();
            }
            return true;
        }

    }
}
