using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace DotNet.Utilities.EncryptCollection
{
    public static class UTF8code
    {
        // Methods
        public static string DecodeUTF8(string paramsstring)
        {
            return HttpContext.Current.Server.UrlDecode(paramsstring);
        }
        public static string DeRepleaseDES(string paramsstring)
        {
            return paramsstring.Replace("$thisisarepleaseforaddletter$", "+");
        }
        public static string EncodeUTF8(string paramsstring)
        {
            return HttpContext.Current.Server.UrlEncode(paramsstring);
        }
        public static string RepleaseDES(string paramsstring)
        {
            return paramsstring.Replace("+", "$thisisarepleaseforaddletter$");
        }
    }
}
