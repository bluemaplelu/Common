using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;

namespace QHW.Common
{
    public class AspNetPager : Control
    {
        #region 属性
        private string css = "paginator";

        /// <summary>
        /// 分页容器DIV样式(默认:the_pages)
        /// </summary>
        public string PageDivCSS
        {
            get { return css; }
            set { css = value; }
        }

        private int totalEntriesCount;
        /// <summary>
        /// 总条目数目
        /// </summary>
        public int RecordCount
        {
            get
            {
                return this.totalEntriesCount;
            }
            set { this.totalEntriesCount = value; }
        }
        private int _countPerPage = 1;
        /// <summary>
        /// 每页条目数目
        /// </summary>
        public int PageSize
        {
            get
            {
                return this._countPerPage;
            }
            set
            {
                this._countPerPage = value;
            }
        }
        private int _pageNo;
        /// <summary>
        /// 页码
        /// </summary>
        public int CurrentPageIndex
        {
            get
            {
                if (_pageNo == 0)
                {
                    _pageNo = Common.RequestHelper.GetInt("page", 1);
                }
                return _pageNo;
            }
            set
            {
                _pageNo = value;
            }
        }
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                if (RecordCount == 0)
                    return 1;
                return (int)Math.Ceiling((double)RecordCount / (double)PageSize);
            }
        }

        private bool _DisplayTotalPageCount = false;

        /// <summary>
        /// 显示总页数
        /// </summary>
        public bool DisplayTotalPageCount
        {
            get { return _DisplayTotalPageCount; }
            set { _DisplayTotalPageCount = value; }
        }


        private string _UrlRewritePattern = string.Empty;

        public string UrlRewritePattern
        {
            get { return _UrlRewritePattern; }
            set { _UrlRewritePattern = value; }
        }

        private bool isShowInput = false;
        /// <summary>
        /// 是否现实输入框
        /// </summary>
        public bool IsShowInput { get { return isShowInput; } set { isShowInput = value; } }

        private bool isShowPreNext = true;
        /// <summary>
        /// 是否现实输入框
        /// </summary>
        public bool IsShowPreNext { get { return isShowPreNext; } set { isShowPreNext = value; } }

        #endregion

        protected override void Render(HtmlTextWriter writer)
        {
            this.RenderQueryString(writer);
        }
        /// <summary>
        /// 输出页码块
        /// </summary>
        /// <param name="no">页码</param>
        /// <param name="writer">writer</param>
        private void RenderPageNoBlock(int no, HtmlTextWriter writer)
        {
            if (no == this.CurrentPageIndex)
            {
                writer.Write(string.Format("<span class=\"this-page\">{0}</span>", no.ToString()));
            }
            else
            {
                writer.Write(string.Format("<a href=\"{0}\">{1}</a>", string.Format(UrlRewritePattern, no), no.ToString()));
            }
        }
        private void RenderPreDotBlock(HtmlTextWriter writer)
        {
            if (this.CurrentPageIndex >= 5)
            {
                writer.Write("<a disabled=\"disabled\">...</a>");
            }
        }
        private void RenderPostDotBlock(HtmlTextWriter writer)
        {
            if (this.CurrentPageIndex + 4 <= this.PageCount)
            {
                writer.Write("<a disabled=\"disabled\">...</a>");
            }
        }
        private void RenderPageMiddleBlock(HtmlTextWriter writer)
        {
            if (this.CurrentPageIndex >= 4)
            {
                if (this.CurrentPageIndex <= this.PageCount - 3)
                {
                    for (int i = this.CurrentPageIndex - 2; i <= this.CurrentPageIndex + 2; i++)
                    {
                        this.RenderPageNoBlock(i, writer);
                    }
                }
                else
                {
                    for (int i = this.PageCount - 5; i <= this.PageCount - 1; i++)
                    {
                        this.RenderPageNoBlock(i, writer);
                    }
                }
            }
            else
            {
                for (int i = 2; i <= 6; i++)
                {
                    this.RenderPageNoBlock(i, writer);
                }
            }
        }
        private void RenderPrevPageBlock(HtmlTextWriter writer)
        {
            if (this.CurrentPageIndex > 1)
            {
                writer.Write(string.Format("<a class=\"firstbg\" href=\"{0}\">上一页</a>", string.Format(UrlRewritePattern, (this.CurrentPageIndex - 1))));
            }
            else
            {
                writer.Write("<a class=\"firstbg\" disabled=\"disabled\">上一页</a>");
            }
        }
        private void RenderNextPageBlock(HtmlTextWriter writer)
        {
            if (this.CurrentPageIndex < this.PageCount)
            {
                writer.Write(string.Format("<a class=\"firstbg\" href=\"{0}\">下一页</a>", string.Format(UrlRewritePattern, (this.CurrentPageIndex + 1))));
            }
            else
            {
                writer.Write("<a class=\"firstbg\" disabled=\"disabled\">下一页</a>");
            }
        }
        private void RenderQueryString(HtmlTextWriter writer)
        {
            if (this.PageCount > 1)
            {
                //int prev = PageNo - 1;
                //int next = PageNo + 1;
                writer.Write(string.Format("<div class='{0}'><div>", PageDivCSS));
                if (DisplayTotalPageCount)
                {
                    writer.Write(string.Format("<span class=\"num\">共{0}页</span>", PageCount));
                }
                if (IsShowPreNext) this.RenderPrevPageBlock(writer);
                if (this.PageCount <= 9)
                {
                    for (int i = 1; i <= this.PageCount; i++)
                    {
                        this.RenderPageNoBlock(i, writer);
                    }
                }
                else
                {
                    this.RenderPageNoBlock(1, writer);
                    this.RenderPreDotBlock(writer);
                    this.RenderPageMiddleBlock(writer);
                    this.RenderPostDotBlock(writer);
                    this.RenderPageNoBlock(this.PageCount, writer);
                }
                if (IsShowPreNext) this.RenderNextPageBlock(writer);
                if (IsShowInput)
                {
                    writer.Write("<span class=\"num fd-left mt5\">页码:</span> <input class=\"txt8 fd-left mr5\" id=\"index\" name=\"\" type=\"text\"/><a class=\"gray-btn fd-left\" href=\"#\" onclick=\"javascript:goPage($('#index').val())\"class=\"ok\">转到</a>");
                    writer.Write(@"
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
                writer.Write("</div></div>");

            }
        }
    }

    public class SimplePager : Control
    {
        #region 属性
        private string css = "paginator";

        /// <summary>
        /// 分页容器DIV样式(默认:the_pages)
        /// </summary>
        public string PageDivCSS
        {
            get { return css; }
            set { css = value; }
        }

        private int totalEntriesCount;
        /// <summary>
        /// 总条目数目
        /// </summary>
        public int RecordCount
        {
            get
            {
                return this.totalEntriesCount;
            }
            set { this.totalEntriesCount = value; }
        }
        public string RecordCss
        {
            get;
            set;
        }
        private int _countPerPage = 1;
        /// <summary>
        /// 每页条目数目
        /// </summary>
        public int PageSize
        {
            get
            {
                return this._countPerPage;
            }
            set
            {
                this._countPerPage = value;
            }
        }
        private int _pageNo;
        /// <summary>
        /// 页码
        /// </summary>
        public int CurrentPageIndex
        {
            get
            {
                if (_pageNo == 0)
                {
                    if (HttpContext.Current.Request.QueryString["page"] != null)
                    {
                        int.TryParse(HttpContext.Current.Request.QueryString["page"].ToString(), out this._pageNo);
                    }
                    if (this._pageNo == 0) this._pageNo = 1;
                }
                return _pageNo > PageCount ? PageCount : _pageNo;
            }
            set
            {
                _pageNo = value;
            }
        }
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                if (RecordCount == 0)
                    return 1;
                return (int)Math.Ceiling((double)RecordCount / (double)PageSize);
            }
        }

        private string _UrlRewritePattern;

        public string UrlRewritePattern
        {
            get { return _UrlRewritePattern; }
            set { _UrlRewritePattern = value; }
        }

        #endregion

        protected override void Render(HtmlTextWriter writer)
        {
            this.RenderQueryString(writer);
        }

        private void RenderLastPageBlock(HtmlTextWriter writer)
        {
            if (this.CurrentPageIndex > 1)
            {
                writer.Write(string.Format("<a href=\"{0}\">上一页</a>", string.Format(UrlRewritePattern, (this.CurrentPageIndex - 1))));
            }
            else
            {
                writer.Write("<a class=\"this-page\">上一页</a>");
            }
        }
        private void RenderNextPageBlock(HtmlTextWriter writer)
        {
            if (this.CurrentPageIndex < this.PageCount)
            {
                writer.Write(string.Format("<a href=\"{0}\">下一页</a>", string.Format(UrlRewritePattern, (this.CurrentPageIndex + 1))));
            }
            else
            {
                writer.Write("<a class=\"this-page\" href='javascript:void(0)'>下一页</a>");
            }
        }
        private void RenderQueryString(HtmlTextWriter writer)
        {
            if (this.PageCount > 1)
            {
                writer.Write(string.Format("<div class='{0}'>", PageDivCSS));
                //writer.Write(string.Format("<div><span><span  style='{2}'>共<strong>{0}</strong>页，</span>当前第<strong>{1}</strong>页</span>", PageCount, CurrentPageIndex, RecordCss));
                this.RenderLastPageBlock(writer);
                this.RenderNextPageBlock(writer);
                writer.Write("</div>");
            }
        }
    }
}
