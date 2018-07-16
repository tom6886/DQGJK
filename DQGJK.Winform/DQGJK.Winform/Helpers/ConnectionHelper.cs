using DQGJK.Winform.Models;
using System;
using System.Linq;

namespace DQGJK.Winform.Helpers
{
    internal class ConnectionHelper
    {
        internal static void OffLine(string clientCode)
        {
            using (DBContext db = new DBContext())
            {
                db.ExceptionLog.Add(new ExceptionLog(clientCode, "", ExceptionType.offline));
                db.SaveChanges();
            }
        }

        internal static void OnLine(string clientCode)
        {
            using (DBContext db = new DBContext())
            {
                ExceptionLog log = db.ExceptionLog.Where(q => q.ClientCode.Equals(clientCode)
                                        && q.Type == ExceptionType.offline).OrderByDescending(q => q.CreateTime).FirstOrDefault();

                if (log == null || log.EndTime != null) { return; }

                log.EndTime = DateTime.Now;

                db.SaveChanges();
            }
        }
    }
}
