using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sticky.Models.Druid;

namespace Sticky.Repositories.Script
{
    public interface IKafkaClient
    {
        Task SendMessage(DruidData message);
    }

}

