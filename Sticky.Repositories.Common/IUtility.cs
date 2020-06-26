using System;
using System.Collections.Generic;
using System.Text;

namespace Sticky.Repositories.Common
{
    public interface IUtility
    {
        /// <summary>
        /// sometimes there is 2 dots in address and the default aut getter in .net does not separate the main part
        /// of the address. this method is for extracting the parent domain
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        string GetTopDomainFromAddress(string address);
        /// <summary>
        /// returns a plain text of a base64 input.
        /// </summary>
        /// <param name="base64EncodedData">
        /// base64 input 
        /// </param>
        /// <param name="hasExtraChars">
        /// indicates the input is changed to be not readable or not
        /// </param>
        /// <returns></returns>
        string Base64Decode(string base64EncodedData, bool hasExtraChars = false);
        /// <summary>
        /// returns base64 encoded version of text.
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="hasExtraChars">set it true if for changing the result so others can not read it.</param>
        /// <returns></returns>
        string Base64Encode(string plainText, bool hasExtraChars = false);
    }
}
