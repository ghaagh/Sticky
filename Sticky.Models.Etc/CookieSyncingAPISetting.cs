using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Models.Etc
{
    public class CookieSyncingAPISetting
    {
        /// <summary>
        /// returns Cookie Name for using in Cookie Setting.
        /// </summary>
        public string CookieName { get; set; }
        /// <summary>
        /// return Cookie domain for using in Cookie Setting.
        /// </summary>
        public string CookieDomain { get; set; }
    }
}
