using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sticky.Application.CookieSyncing
{
    public class Setting
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
