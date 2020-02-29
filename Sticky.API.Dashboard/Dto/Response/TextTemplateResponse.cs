using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Dto.Dashboard.Response
{
    public class TextTemplateResponseV2 : EmptyResponse
    {
        public List<TextTemplateResult> Result { get; set; } = new List<TextTemplateResult>();
    }
    public class TextTemplateResponse : EmptyResponse
    {
        public List<string> Result { get; set; } = new List<string>();
    }
    public class TextTemplateResult
    {
        public string Body { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
    }
}
