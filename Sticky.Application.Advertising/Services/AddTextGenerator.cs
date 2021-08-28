using StackExchange.Redis;
using Sticky.Infrastructure.Cache;
using Sticky.Infrastructure.Cache.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky.Application.Advertising.Services
{
    public class AddTextGenerator: IAdTextGenerator
    {
        private readonly IMultipleCache<SegmentCache> _segmentCache;
        public AddTextGenerator(IMultipleCache<SegmentCache> segmentCache)
        {
            _segmentCache = segmentCache;

        }
        public string Clean(string name)
        {
            if (name.Contains("مدل"))
            {
                name = name.Split("مدل")[0];
            }
            if (name.Contains("طرح"))
            {
                name = name.Split("طرح")[0];
            }
            if (name.Contains(" کد "))
            {
                name = name.Split(" کد ")[0];
            }
            return name;
        }
        public async Task<(string finalName,long templateId)>  CreateAdvertisingText(long segmentId, string name, int? price)
        {
            if (price == null || price == 0)
                return (name,0);

            name = Clean(name);
            var templates = (await _segmentCache.GetAsync(segmentId.ToString())).Templates;
            List<TemplateCache> acceptedTemplates = new List<TemplateCache>();
            var count = templates.Count();
            if (count == 0)
                return (name, 0);
            foreach (var item in templates)
            {
                if ((item.MaxPrice == 0 && item.MinPrice == 0) || 
                    (item.MaxPrice == 0 && item.MinPrice != 0 && price >= item.MinPrice) || 
                    (item.MinPrice == 0 && item.MaxPrice != 0 && price <= item.MaxPrice) || 
                    (item.MinPrice != 0 && item.MaxPrice != 0 && price <= item.MaxPrice && price >= item.MinPrice))
                    acceptedTemplates.Add(item);

            }
            if (acceptedTemplates.Count() == 0)
                return (name, 0);
            var finalTemplate = acceptedTemplates.OrderBy(c => Guid.NewGuid()).FirstOrDefault();
            var finalName = finalTemplate.Template.Replace("@Name", name);
                finalName = finalName.Replace("@Price", ((price ?? 0) / 10).ToString("N0"));
            return (finalName,finalTemplate.Id);


        }
    }
}
