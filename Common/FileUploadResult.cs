using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QHW.Common
{
    #region 文件上传结果
    /// <summary>
    /// 文件上传结果
    /// </summary>
    public enum FileUploadResult
    {
        文件上传成功,
        文件为空,
        文件大小超过限制,
        文件扩展名不允许,
        文件上传异常,
    }
    #endregion
}
