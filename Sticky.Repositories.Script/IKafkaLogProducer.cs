using Sticky.Dto.Script.Request;
using Sticky.Models.Etc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sticky.Repositories.Script
{
    public interface IKafkaLogProducer
    {
        Task GenerateProductLogFromId(string statType, ModifyProductLog productLog,string host);
        Task GenerateProductLogFromProducts(string statType, ProductLog productLog,string host);
        Task GeneratePageLog(PageLogRequest pageLogRequest, string host);
    }
}
