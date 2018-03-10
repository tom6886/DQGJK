using System;
using System.IO;

namespace DQGJK.Service
{
    public class LogHelper
    {
        public static void WriteLog(string matter)
        {
            WriteLog(matter, null, null);
        }

        public static void WriteLog(string matter, string message, string stack)
        {
            DateTime now = DateTime.Now;

            string fileName = string.Format(@"C:\DQGJKLogs\\{0}.txt", now.ToString("yyyyMMdd"));

            using (StreamWriter sw = new StreamWriter(fileName, true))
            {
                sw.WriteLine("==============================================================");
                sw.WriteLine("时间：" + now.ToString("yyyy-MM-dd HH:mm:ss"));
                sw.WriteLine("事件：" + matter);
                if (!string.IsNullOrEmpty(message)) { sw.WriteLine("内容：" + message); }
                if (!string.IsNullOrEmpty(stack)) { sw.WriteLine("追踪：" + stack); }

                sw.Flush();
                sw.Close();
            }
        }
    }
}
