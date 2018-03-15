using DQGJK.Message;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace DQGJK.Winform.Handlers
{
    internal class C0Handler : IMessageHandler
    {
        public C0Handler(string UID, RecieveMessage Message)
        {
            _UID = UID;
            _Message = Message;
        }

        public RecieveMessage _Message { get; set; }

        public string _UID { get; set; }

        public void Handle()
        {
            MongoHandler.Save(new B0C0Data(_Message));

            Response();

            UpdateCabinet();
        }

        private void Response()
        {
            SendMessage res = new SendMessage();
            res.ClientCode = _Message.ClientCode;
            res.CenterCode = _Message.CenterCode;
            res.SendTime = DateTime.Now;
            res.Serial = 0;
            res.FunctionCode = "C0";

            Main.listener.Send(_UID, res.ToByte());
        }

        private void UpdateCabinet()
        {
            using (DBContext db = new DBContext())
            {
                Station _station = db.Station.Where(q => q.Code.Equals(_Message.ClientCodeStr)).FirstOrDefault();

                if (_station == null) { return; }

                DateTime now = DateTime.Now;

                _station.ModifyTime = now;

                db.Entry(_station).State = EntityState.Modified;

                List<Cabinet> _cabinets = db.Cabinet.Where(q => q.StationCode.Equals(_Message.ClientCodeStr)).ToList();

                List<B0C0Element> data = (List<B0C0Element>)_Message.Data;

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

                    cabinet.Humidity = item.Humidity;
                    cabinet.Temperature = item.Temperature;
                    cabinet.RelayOne = item.State.RelayOne;
                    cabinet.RelayTwo = item.State.RelayTwo;
                    cabinet.HumidityAlarm = item.State.HumidityAlarm;
                    cabinet.TemperatureAlarm = item.State.TemperatureAlarm;
                    cabinet.Dehumidify = item.State.Dehumidify;

                    if (isNull)
                        db.Cabinet.Add(cabinet);
                    else
                        db.Entry(cabinet).State = EntityState.Modified;
                }

                db.SaveChanges();
            }
        }
    }
}
