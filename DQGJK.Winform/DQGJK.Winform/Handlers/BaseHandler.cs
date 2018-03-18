﻿using DQGJK.Message;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DQGJK.Winform.Handlers
{
    internal abstract class BaseHandler<T> where T : IElement
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
                        cabinet.StationCode = _Message.ClientCodeStr;
                        cabinet.Status = Status.enable;
                    }

                    cabinet.ModifyTime = now;

                    if (!SetCabinet(item, ref cabinet)) { continue; };

                    if (isNull)
                        db.Cabinet.Add(cabinet);
                    else
                        db.Entry(cabinet).State = EntityState.Modified;

                    db.SaveChanges();
                }
            }
        }

        public abstract bool SetCabinet(T element, ref Cabinet cabinet);
    }
}