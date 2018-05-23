using DQGJK.Message;
using DQGJK.Winform.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DQGJK.Winform.Handlers
{
    public abstract class BaseHandler<T> where T : IElement
    {
        public void UpdateCabinet(RecieveMessage _Message)
        {
            using (DBContext db = new DBContext())
            {
                Station _station = db.Station.Where(q => q.Code.Equals(_Message.ClientCodeStr)).FirstOrDefault();

                if (_station == null) { return; }

                DateTime now = DateTime.Now;

                _station.ModifyTime = now;

                db.Entry(_station).State = EntityState.Modified;

                if (_Message.Data == null) { db.SaveChanges(); return; }

                List<Cabinet> _cabinets = db.Cabinet.Where(q => q.StationCode.Equals(_Message.ClientCodeStr)).ToList();

                List<T> data = (List<T>)_Message.Data;

                foreach (var item in data)
                {
                    Cabinet cabinet = _cabinets.Where(q => q.Code.Equals(item.Code)).FirstOrDefault();

                    bool isNull = cabinet == null;

                    if (isNull)
                    {
                        cabinet = new Cabinet();
                        cabinet.Name = item.Code;
                        cabinet.Code = item.Code;
                        if (!item.Code.Equals("FF")) { cabinet.Sort = Convert.ToInt32(item.Code, 16); }
                        cabinet.StationCode = _Message.ClientCodeStr;
                        cabinet.Status = Status.enable;
                    }

                    if (!SetCabinet(item, ref cabinet)) { continue; };

                    if (isNull)
                        db.Cabinet.Add(cabinet);
                    else
                        db.Entry(cabinet).State = EntityState.Modified;
                }

                db.SaveChanges();
            }
        }

        public abstract bool SetCabinet(T element, ref Cabinet cabinet);

        public void UpdateOperate(RecieveMessage _Message, string FunctionCode)
        {
            using (DBContext db = new DBContext())
            {
                List<Operate> operate = db.Operate.Where(q => q.ClientCode.Equals(_Message.ClientCodeStr)
                                            && q.State == OperateState.Sended && q.FunctionCode.Equals(FunctionCode)).ToList();

                foreach (var item in operate)
                {
                    item.State = OperateState.Done;
                    db.Entry(item).State = EntityState.Modified;
                }

                db.SaveChanges();
            }
        }
    }
}