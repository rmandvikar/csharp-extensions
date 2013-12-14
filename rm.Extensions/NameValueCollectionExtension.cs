using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace rm.Extensions
{
    /// <summary>
    /// NameValueCollection extensions.
    /// </summary>
    public static class NameValueCollectionExtension
    {
        /// <summary>
        /// Get query string for name value collection.
        /// </summary>
        public static string ToQueryString(this NameValueCollection collection,
            bool prefixQuestionMark = true)
        {
            collection.NullArgumentCheck();
            if (collection.Keys.Count == 0)
            {
                return "";
            }
            var buffer = new StringBuilder();
            if (prefixQuestionMark)
            {
                buffer.Append("?");
            }
            var append = false;
            for (int i = 0; i < collection.Keys.Count; i++)
            {
                var key = collection.Keys[i];
                var values = collection.GetValues(key);
                key.NullCheck();
                values.NullCheck();
                foreach (var value in values)
                {
                    if (append)
                    {
                        buffer.Append("&");
                    }
                    append = true;
                    buffer.AppendFormat("{0}={1}", key.UrlEncode(), value.UrlEncode());
                }
            }
            return buffer.ToString();
        }
    }
}
