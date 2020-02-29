using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky.Dto.Dashboard.Response
{
    public class CategoryReponse : EmptyResponse
    {
        public IEnumerable<CategorytResult> Result { get; set; }
    }
    public class CategorytResult
    {
        public int Id { get; set; }
        public int HostId { get; set; }
        public string CategoryName { get; set; }
        public long Counter { get; set; }
    }
}
  
