using Microsoft.EntityFrameworkCore;
using Sticky.Models.Context;
using Sticky.Models.Etc;
using Sticky.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky.Repositories.Advertisement.Implementions
{
    public class AwesomeTextGenerator : IAwesomeTextGenerator
    {
        private readonly IRedisCache _redisCache;
        public AwesomeTextGenerator(IRedisCache redisCache)
        {
            _redisCache = redisCache;

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
        public async Task<AdvertisingText> CreateAdvertisingText(int segmentId, string name, int? price)
        {
            if (price == null || price == 0)
                return new AdvertisingText() { ProductText = name, TemplateText = "NoTemplate" };
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
            var database = _redisCache.GetDatabase(RedisDatabases.TextTemplateV2);
            var templates = await database.HashGetAllAsync(segmentId.ToString());
            List<string> acceptedTemplates = new List<string>();
            var count = templates.Count();
            if (count == 0)
                return new AdvertisingText() { ProductText = name, TemplateText = "NoTemplate" };
            foreach (var item in templates)
            {
                var templateData = item.Value.ToString().Split("%%%");
                var templateBody = templateData[0];
                var minprice = int.Parse(templateData[1]);
                var maxPrice = int.Parse(templateData[1]);
                if ((maxPrice == 0 && minprice == 0) || (maxPrice == 0 && minprice != 0 && price >= minprice) || (minprice == 0 && maxPrice != 0 && price <= maxPrice) || (minprice != 0 && maxPrice != 0 && price <= maxPrice && price >= minprice))
                    acceptedTemplates.Add(templateBody);

            }
            if (acceptedTemplates.Count() == 0)
                return new AdvertisingText() { ProductText = name, TemplateText = "NoTemplate" };
            var templateFinalText = acceptedTemplates.OrderBy(c => Guid.NewGuid()).FirstOrDefault().ToString();
            var templateCp = templateFinalText;
            templateFinalText = templateFinalText.Replace("@Name", name);
            if (templateFinalText.Contains("تومان") || templateFinalText.Contains("تومن"))
                templateFinalText = templateFinalText.Replace("@Price", ((price ?? 0) / 10).ToString("N0"));
            else
                templateFinalText = templateFinalText.Replace("@Price", ((price ?? 0)).ToString("N0"));
            return new AdvertisingText() { ProductText = templateFinalText, TemplateText = templateCp };


        }
        public async Task Initial(string connectionString)
        {

            try
            {
                DbContextOptionsBuilder<StickyDbContext> optionsBuilder = new DbContextOptionsBuilder<StickyDbContext>();
                optionsBuilder.UseSqlServer(connectionString);
                using (StickyDbContext _db = new StickyDbContext(optionsBuilder.Options))
                {
                    var newdatabase = _redisCache.GetDatabase(RedisDatabases.TextTemplateV2);
                    var list = await _db.ProductTextTemplates.GroupBy(c => c.SegmentId).ToListAsync();

                    foreach (var item in list)
                    {
                        await newdatabase.KeyDeleteAsync(item.Key.ToString());
                        foreach (var zitem in item.ToList())
                        {
                            await newdatabase.HashSetAsync(item.Key.ToString(), zitem.Template + "%%%" + (zitem.MinPrice ?? 0) + "%%%" + (zitem.MaxPrice ?? 0), zitem.Template + "%%%" + (zitem.MinPrice ?? 0) + "%%%" + (zitem.MaxPrice ?? 0));
                        }
                    }
                }

            }
            catch
            {
            }



        }
    }
}
