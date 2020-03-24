using Sticky.Models.Etc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sticky.Repositories.Advertisement
{
    public interface IAwesomeTextGenerator
    {
        string Clean(string name);
        Task<AdvertisingText> CreateAdvertisingText(int segmentId,string name, int? price);
    }
}
