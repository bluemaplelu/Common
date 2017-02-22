using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace DotNet.Utilities
{
    public class CookieHelper
    {
        /// <summary>
        /// 写Cookie
        /// </summary>
        /// <param name="name">名</param>
        /// <param name="value">值</param>
        /// <param name="domain">域</param>
        /// <param name="expiration">过期时间</param>
        public static void WriteCookie(string name, string value, string domain, DateTime? expiration)
        {
            WriteCookie(name, value, domain, expiration, false);
        }

        /// <summary>
        /// 写Cookie
        /// </summary>
        /// <param name="name">名</param>
        /// <param name="value">值</param>
        /// <param name="domain">域</param>
        /// <param name="expiration">过期时间</param>
        /// <param name="HttpOnly">是否HttpOnly</param>
        public static void WriteCookie(string name, string value, string domain, DateTime? expiration, bool HttpOnly)
        {
            HttpCookie ck = new HttpCookie(name, HttpUtility.UrlEncode(value));
            ck.HttpOnly = HttpOnly;
            if (!string.IsNullOrEmpty(domain))
            {
                ck.Domain = domain;
            }
            if (expiration.HasValue)
            {
                ck.Expires = expiration.Value;
            }
            System.Web.HttpContext.Current.Response.Cookies.Add(ck);
        }

        /// <summary>
        /// 移除Cookie
        /// </summary>
        /// <param name="name">名</param>
        /// <param name="domain">域</param>
        public static void RemoveCookie(string name, string domain)
        {
            if (System.Web.HttpContext.Current.Request.Cookies[name] != null)
            {
                HttpCookie cookie = new HttpCookie(name, "");
                cookie.Expires = new DateTime(1999, 12, 1);
                if (!string.IsNullOrEmpty(domain))
                {
                    cookie.Domain = domain;
                }
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="name">名</param>
        /// <returns>值</returns>
        public static string GetCookie(string name)
        {
            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[name];
            if (cookie != null)
            {
                return HttpUtility.UrlDecode(cookie.Value);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 添加一个新的Cookie到HttpCookes集合
        /// </summary>
        /// <param name="strCookName">Cookie的名称</param>
        /// <param name="strCookValue">Cookie的值</param>
        public static void AddCookies(string strCookName, string strCookValue)
        {
            if (System.Web.HttpContext.Current.Request.Browser.Cookies == true)
            {
                HttpCookie Cookies = new HttpCookie(strCookName, strCookValue);

                System.Web.HttpContext.Current.Response.AppendCookie(Cookies);

            }
            else
            {
                System.Web.HttpContext.Current.Session[strCookName] = strCookValue;
            }
        }

        /// <summary>
        /// 添加一个Cookie到HttpCookes集合并设置其过期时间
        /// </summary>
        /// <param name="strCookName"></param>
        /// <param name="strCookValue"></param>
        /// <param name="dtExpires"></param>
        public static void AddCookies(string strCookName, string strCookValue, DateTime dtExpires)
        {
            if (System.Web.HttpContext.Current.Request.Browser.Cookies == true)
            {
                HttpCookie myCookies = new HttpCookie(strCookName);
                myCookies.Value = strCookValue;
                myCookies.Expires = dtExpires;
                System.Web.HttpContext.Current.Response.Cookies.Add(myCookies);
            }
            else
            {
                System.Web.HttpContext.Current.Session[strCookName] = strCookValue;
            }
        }

        /// <summary>
        /// 删除指定的Cookie
        /// </summary>
        /// <param name="strCookName">Cookie名称</param>
        public static void DelCookies(string strCookName)
        {
            if (System.Web.HttpContext.Current.Request.Browser.Cookies == true)
            {
                if (System.Web.HttpContext.Current.Request.Cookies[strCookName] != null)
                {
                    HttpCookie myCookie = new HttpCookie(strCookName);
                    myCookie.Expires = DateTime.Now.AddDays(-1d);
                    System.Web.HttpContext.Current.Response.Cookies.Add(myCookie);
                }
            }
            else
            {
                System.Web.HttpContext.Current.Session.Contents.Remove(strCookName);
            }
        }
        /// <summary>
        /// 获取指定Cookie的值
        /// </summary>
        /// <param name="strCookName">Cookie名称</param>
        /// <returns></returns>
        public static string GetCookieValue(string strCookName)
        {
            if (System.Web.HttpContext.Current.Request.Browser.Cookies == true)
            {
                if (HttpContext.Current.Request.Cookies[strCookName] != null)
                {
                    return HttpContext.Current.Request.Cookies[strCookName].Value;
                }
                else
                {
                    return String.Empty;
                }
            }
            else
            {
                if (HttpContext.Current.Session[strCookName] != null)
                {
                    return System.Web.HttpContext.Current.Session[strCookName].ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
        }
    }
}
