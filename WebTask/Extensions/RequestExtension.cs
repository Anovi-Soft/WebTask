using System.Web;

namespace WebTask.Extensions
{
    public static class RequestExtension
    {
        public static string GetClientIp(this HttpRequestBase request)
        {
            var addr = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ??
                    request.ServerVariables["REMOTE_ADDR"];
            return addr.Split(',')[0].Trim();
        }
        public static string GetClientIp(this HttpRequest request)
        {
            var addr = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ??
                    request.ServerVariables["REMOTE_ADDR"];
            return addr.Split(',')[0].Trim();
        }
    }
}