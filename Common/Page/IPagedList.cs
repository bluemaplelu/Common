using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHW.Common.Page
{
    /// <summary>
    /// 分页接口类
    /// 作者： 闫波
    /// 时间： 2016-01-15
    /// </summary>
    public interface IPagedList
    {
        int CurrentPageIndex { get; set; }
        int PageSize { get; set; }
        int TotalItemCount { get; set; }
    }
}
