using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DotNet.Utilities
{
    public class PublicSqlData
    {
        #region 分页
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString">数据连接</param>
        /// <param name="tblName">表名</param>
        /// <param name="fldName">取得字段名</param>
        /// <param name="pageSize">每页个数</param>
        /// <param name="pageIndex">当前页面</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="orderType">排序方法，false为升序，true为降序</param>
        /// <param name="strCondition">查询条件,不需where,以And开始</param>
        /// <param name="pageCount">总页数</param>
        /// <param name="recordCount">总记录数</param>
        /// <returns></returns>
        public static DataSet SimplePager(
        string connString,
        string tblName,
        string fldName,
        int pageSize,
        int pageIndex,
        string orderField,
        bool orderType,
        string strCondition,
        out int pageCount,
        out int recordCount,
            int expireMin,
            int defaultrecordCount
        )
        {
            string key = MD5.Md5En(string.Format("{0}-{1}", tblName, strCondition));
            recordCount = 1;
            if (expireMin > 0)
            {
                int count = Common.TypeParse.ObjectToInt(Common.DotNetCacheManager.Get(key), 1);
                recordCount = count;
            }

            SqlParameter pageCountOut = new SqlParameter("@pageCount", SqlDbType.Int);
            SqlParameter countOut = new SqlParameter("@Counts", defaultrecordCount > 0 ? defaultrecordCount : recordCount);
            pageCountOut.Direction = ParameterDirection.InputOutput;
            countOut.Direction = ParameterDirection.InputOutput;

            DataSet ds = SqlHelper.ExecuteDataset(
                connString,
                CommandType.StoredProcedure,
                "SimplePager",
                new SqlParameter("@tblName", tblName),
                new SqlParameter("@fldName", fldName ?? "*"),
                new SqlParameter("@pageSize", pageSize),
                new SqlParameter("@page", pageIndex),
                new SqlParameter("@ID", orderField),
                new SqlParameter("@Sort", orderType),
                new SqlParameter("@strCondition", strCondition ?? ""),
                pageCountOut,
                countOut
            );

            pageCount = Common.TypeParse.ObjectToInt(pageCountOut.Value, 0);
            recordCount = Common.TypeParse.ObjectToInt(countOut.Value, 0);
            if (expireMin > 0)
            {
                Common.DotNetCacheManager.Add(key, countOut.Value, expireMin);
            }
            return ds;
        }
        public static DataSet SimplePager(
        string connString,
        string tblName,
        string fldName,
        int pageSize,
        int pageIndex,
        string orderField,
        bool orderType,
        string strCondition,
        out int pageCount,
        out int recordCount,
            int expireMin
        )
        {
            return SimplePager(connString,
            tblName,
            fldName,
            pageSize,
            pageIndex,
            orderField,
            orderType,
            strCondition,
            out pageCount,
            out recordCount,
            expireMin,
            0);
        }
        public static DataSet SimplePager(
        string connString,
        string tblName,
        string fldName,
        int pageSize,
        int pageIndex,
        string orderField,
        bool orderType,
        string strCondition,
        out int pageCount,
        out int recordCount
        )
        {
            return SimplePager(connString,
            tblName,
            fldName,
            pageSize,
            pageIndex,
            orderField,
            orderType,
            strCondition,
            out pageCount,
            out recordCount,
            15,
            0);
        }

        #endregion
    }
}
