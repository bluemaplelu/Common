using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Text;
using System.Web.Mvc.Html;

namespace DotNet.Utilities
{
    public static class MVCPager
    {
        #region 属性
        /// <summary>
        /// 分页容器DIV样式(默认:the_pages)
        /// </summary>
        public static string PageDivCSS { get; set; }
        /// <summary>
        /// 总条目数目
        /// </summary>
        public static int RecordCount { get; set; }
        /// <summary>
        /// 每页条目数目
        /// </summary>
        public static int PageSize { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public static int CurrentPageIndex { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public static int PageCount
        {
            get
            {
                if (RecordCount == 0)
                    return 1;
                return (int)Math.Ceiling((double)RecordCount / (double)PageSize);
            }
        }


        /// <summary>
        /// 显示总页数
        /// </summary>
        public static bool DisplayTotalPageCount { get; set; }



        public static string UrlRewritePattern { get; set; }

        /// <summary>
        /// 是否现实输入框
        /// </summary>
        public static bool IsShowInput { get; set; }

        private static bool isShowPreNext = true;
        /// <summary>
        /// 是否现实输入框
        /// </summary>
        public static bool IsShowPreNext { get { return isShowPreNext; } set { isShowPreNext = value; } }

        #endregion

        public static HtmlString RenderPager(int _RecordCount, int _PageSize, string _UrlRewritePattern, bool _IsShowInput)
        {
            return RenderPager(PageDivCSS, _RecordCount, _PageSize, CurrentPageIndex, DisplayTotalPageCount, _UrlRewritePattern, _IsShowInput, IsShowPreNext);
        }
        public static HtmlString RenderPager(int _RecordCount, int _PageSize, string _UrlRewritePattern)
        {
            return RenderPager(PageDivCSS, _RecordCount, _PageSize, CurrentPageIndex, DisplayTotalPageCount, _UrlRewritePattern, IsShowInput, IsShowPreNext);
        }

        public static HtmlString RenderPager(int _RecordCount, int _PageSize, int _CurrentPageIndex, string _UrlRewritePattern)
        {
            return RenderPager(PageDivCSS, _RecordCount, _PageSize, _CurrentPageIndex, DisplayTotalPageCount, _UrlRewritePattern, IsShowInput, IsShowPreNext);
        }

        public static HtmlString RenderPager(string _PageDivCSS, int _RecordCount, int _PageSize, int _CurrentPageIndex, bool _DisplayTotalPageCount, string _UrlRewritePattern, bool _IsShowInput, bool _IsShowPreNext)
        {
            PageDivCSS = _PageDivCSS;
            RecordCount = _RecordCount;
            PageSize = _PageSize;
            CurrentPageIndex = _CurrentPageIndex;
            DisplayTotalPageCount = _DisplayTotalPageCount;
            UrlRewritePattern = _UrlRewritePattern;
            IsShowInput = _IsShowInput;
            IsShowPreNext = _IsShowPreNext;
            var pagerHtml = new StringBuilder();
            RenderQueryString(pagerHtml);
            return new HtmlString(pagerHtml.ToString());
        }
        static MVCPager() { PageDivCSS = "paginator"; PageSize = 1; CurrentPageIndex = Common.RequestHelper.GetInt("pager", 1); }


        /// <summary>
        /// 输出页码块
        /// </summary>
        /// <param name="no">页码</param>
        /// <param name="writer">writer</param>
        private static void RenderPageNoBlock(int no, StringBuilder writer)
        {
            if (no == CurrentPageIndex)
            {
                writer.Append(string.Format("<span class=\"this-page\">{0}</span>", no.ToString()));
            }
            else
            {
                writer.Append(string.Format("<a href=\"{0}\">{1}</a>", string.Format(UrlRewritePattern, no), no.ToString()));
            }
        }
        private static void RenderPreDotBlock(StringBuilder writer)
        {
            if (CurrentPageIndex >= 5)
            {
                writer.Append("<a disabled=\"disabled\">...</a>");
            }
        }
        private static void RenderPostDotBlock(StringBuilder writer)
        {
            if (CurrentPageIndex + 4 <= PageCount)
            {
                writer.Append("<a disabled=\"disabled\">...</a>");
            }
        }
        private static void RenderPageMiddleBlock(StringBuilder writer)
        {
            if (CurrentPageIndex >= 4)
            {
                if (CurrentPageIndex <= PageCount - 3)
                {
                    for (int i = CurrentPageIndex - 2; i <= CurrentPageIndex + 2; i++)
                    {
                        RenderPageNoBlock(i, writer);
                    }
                }
                else
                {
                    for (int i = PageCount - 5; i <= PageCount - 1; i++)
                    {
                        RenderPageNoBlock(i, writer);
                    }
                }
            }
            else
            {
                for (int i = 2; i <= 6; i++)
                {
                    RenderPageNoBlock(i, writer);
                }
            }
        }
        private static void RenderPrevPageBlock(StringBuilder writer)
        {
            if (CurrentPageIndex > 1)
            {
                writer.Append(string.Format("<a class=\"firstbg\" href=\"{0}\"><</a>", string.Format(UrlRewritePattern, (CurrentPageIndex - 1))));
            }
            else
            {
                writer.Append("<a class=\"firstbg\" disabled=\"disabled\"><</a>");
            }
        }
        private static void RenderNextPageBlock(StringBuilder writer)
        {
            if (CurrentPageIndex < PageCount)
            {
                writer.Append(string.Format("<a class=\"firstbg\" href=\"{0}\">></a>", string.Format(UrlRewritePattern, (CurrentPageIndex + 1))));
            }
            else
            {
                writer.Append("<a class=\"firstbg\" disabled=\"disabled\">></a>");
            }
        }
        private static void RenderQueryString(StringBuilder writer)
        {
            if (PageCount > 1)
            {
                //int prev = PageNo - 1;
                //int next = PageNo + 1;
                writer.Append(string.Format("<div class='{0}'><div>", PageDivCSS));
                if (DisplayTotalPageCount)
                {
                    writer.Append(string.Format("<span class=\"num\">共{0}页</span>", PageCount));
                }
                if (IsShowPreNext) RenderPrevPageBlock(writer);
                if (PageCount <= 9)
                {
                    for (int i = 1; i <= PageCount; i++)
                    {
                        RenderPageNoBlock(i, writer);
                    }
                }
                else
                {
                    RenderPageNoBlock(1, writer);
                    RenderPreDotBlock(writer);
                    RenderPageMiddleBlock(writer);
                    RenderPostDotBlock(writer);
                    RenderPageNoBlock(PageCount, writer);
                }
                if (IsShowPreNext) RenderNextPageBlock(writer);
                if (IsShowInput)
                {
                    writer.Append("<span class=\"num fd-left mt5\">页码:</span> <input class=\"txt8 fd-left mr5\" id=\"index\" name=\"\" type=\"text\"/><a class=\"gray-btn fd-left\" href=\"#\" onclick=\"javascript:goPage($('#index').val())\"class=\"ok\">转到</a>");
                    writer.Append(@"
<script type='text/javascript'>
function goPage(pageindex)
{
    var pageCount = " + PageCount + @";
    if(pageindex==''||isNaN(pageindex)||pageindex<=0||pageindex>pageCount)
    {
        return;
    }
    else
    {
        
        var url = '" + string.Format(UrlRewritePattern, "$pageindex") + @"';
        url = url.replace('$pageindex',pageindex);
        " + (UrlRewritePattern.StartsWith("javascript:") ? "eval(url);" : "location.href = url;") + @"
        
    }
}
</script>
");
                }
                writer.Append("</div></div>");

            }
        }
    }

    public class MVCSimplePager
    {
        #region 属性
        /// <summary>
        /// 分页容器DIV样式(默认:the_pages)
        /// </summary>
        public static string PageDivCSS { get; set; }
        /// <summary>
        /// 总条目数目
        /// </summary>
        public static int RecordCount { get; set; }
        public string RecordCss
        {
            get;
            set;
        }
        /// <summary>
        /// 每页条目数目
        /// </summary>
        public static int PageSize { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public static int CurrentPageIndex { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public static int PageCount
        {
            get
            {
                if (RecordCount == 0)
                    return 1;
                return (int)Math.Ceiling((double)RecordCount / (double)PageSize);
            }
        }

        public static string UrlRewritePattern { get; set; }

        #endregion
        static MVCSimplePager() { PageDivCSS = "paginator"; PageSize = 1; CurrentPageIndex = Common.RequestHelper.GetInt("pager", 1); }

        protected static HtmlString RenderPager(int _RecordCount, int _PageSize, string _UrlRewritePattern)
        {
            return RenderPager(PageDivCSS, _RecordCount, _PageSize, CurrentPageIndex, _UrlRewritePattern);

        }
        protected static HtmlString RenderPager(string _PageDivCSS, int _RecordCount, int _PageSize, int _CurrentPageIndex, string _UrlRewritePattern)
        {
            PageDivCSS = _PageDivCSS;
            RecordCount = _RecordCount;
            PageSize = _PageSize;
            CurrentPageIndex = _CurrentPageIndex;
            UrlRewritePattern = _UrlRewritePattern;
            var pagerHtml = new StringBuilder();
            RenderQueryString(pagerHtml);
            return new HtmlString(pagerHtml.ToString());
        }

        private static void RenderLastPageBlock(StringBuilder writer)
        {
            if (CurrentPageIndex > 1)
            {
                writer.Append(string.Format("<a href=\"{0}\">上一页</a>", string.Format(UrlRewritePattern, (CurrentPageIndex - 1))));
            }
            else
            {
                writer.Append("<a class=\"this-page\">上一页</a>");
            }
        }
        private static void RenderNextPageBlock(StringBuilder writer)
        {
            if (CurrentPageIndex < PageCount)
            {
                writer.Append(string.Format("<a href=\"{0}\">下一页</a>", string.Format(UrlRewritePattern, (CurrentPageIndex + 1))));
            }
            else
            {
                writer.Append("<a class=\"this-page\" href='javascript:void(0)'>下一页</a>");
            }
        }
        private static void RenderQueryString(StringBuilder writer)
        {
            if (PageCount > 1)
            {
                writer.Append(string.Format("<div class='{0}'>", PageDivCSS));
                //writer.Append(string.Format("<div><span><span  style='{2}'>共<strong>{0}</strong>页，</span>当前第<strong>{1}</strong>页</span>", PageCount, CurrentPageIndex, RecordCss));
                RenderLastPageBlock(writer);
                RenderNextPageBlock(writer);
                writer.Append("</div>");
            }
        }
    }
}
