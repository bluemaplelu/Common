/*
 * 说明：HttpMock用于在非web项目中使用httpcontext
 * 模拟HttpContext以便，在Winform,Console,WPF等程序中使原来依赖httpContext的方法能运行
 * 例如：依赖HttpContext的Web项目在开发阶段需要测试，但是测试框架是DLL类库或者Console
 * 无法使用HttpContext，可以使用当前类模拟HttpContext
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Web.SessionState;
namespace ConsoleTest
{

    public class HttpMock
    {
        //mock httpcontext complex
        public static void MockHttpContext()
        {
            var httpRequest = new HttpRequest("", "http://localhost", "");
            var stringWriter = new StringWriter();
            var httpResponse = new HttpResponse(stringWriter);
            var httpContext = new HttpContext(httpRequest, httpResponse);
            var sessionContainer = new HttpSessionStateContainer("id", new SessionStateItemCollection(), new HttpStaticObjectsCollection(),
                10, true, HttpCookieMode.AutoDetect, SessionStateMode.InProc, false);
            httpContext.Items["AspSession"] = typeof(HttpSessionState).GetConstructor(
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                null, System.Reflection.CallingConventions.Standard,
                new[] { typeof(HttpSessionStateContainer) }, null).Invoke(new object[] { sessionContainer });
            HttpContext.Current = httpContext;
        }

        //mock httpContext simple 
        public static void MockHttpContextSimple()
        {
            var httpRequest = new HttpRequest("", "http://localhost", "");
            var stringWriter = new StringWriter();
            var httpResponse = new HttpResponse(stringWriter);
            HttpContext.Current = new HttpContext(httpRequest, httpResponse);
        }
        
    }
}
