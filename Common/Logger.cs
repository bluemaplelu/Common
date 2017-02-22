using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace QHW.Common
{
    public class Logger
    {
        public static TraceSource _ts = new TraceSource("TraceLog");
        public static void Write(string msg)
        {
            _ts.TraceData(TraceEventType.Information, 0, msg);
        }
        public static void Clear(string txtPath)
        {
            _ts.Close();
            if (System.IO.File.Exists(txtPath))
            {
                System.IO.File.Delete(txtPath);
            }
            _ts = new TraceSource("TraceLog");
        }
    }
}
