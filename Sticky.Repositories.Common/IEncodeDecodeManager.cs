
namespace Sticky.Repositories.Common
{
    /// <summary>
    /// a utility used for encoding and decoding texts.
    /// </summary>
    public interface IEncodeDecodeManager
    {
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
