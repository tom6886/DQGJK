using DQGJK.Message;
using DQGJK.Winform.Models;
using System;
using System.Data.Entity;
using System.Linq;

namespace DQGJK.Winform.Handlers
{
    internal class C0Handler : BaseHandler<B0C0Element>, IMessageHandler
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

            UpdateCabinet(_Message);
        }

        public override bool SetCabinet(DBContext db, B0C0Element element, ref Cabinet cabinet)
        {
            if (!element.Valid) { return false; }

            string clientCode = cabinet.StationCode;
            string deviceCode = cabinet.Code;

            if (cabinet.HumidityAlarm != element.State.HumidityAlarm)
                SetExceptionLog(db, cabinet.HumidityAlarm, clientCode, deviceCode, ExceptionType.humidity);

            if (cabinet.TemperatureAlarm != element.State.TemperatureAlarm)
                SetExceptionLog(db, cabinet.TemperatureAlarm, clientCode, deviceCode, ExceptionType.temperature);

            cabinet.Humidity = Convert.ToDecimal(element.Humidity);
            cabinet.Temperature = Convert.ToDecimal(element.Temperature);
            cabinet.HumidityLimit = Convert.ToDecimal(element.HumidityLimit);
            cabinet.TemperatureLimit = Convert.ToDecimal(element.TemperatureLimit);
            cabinet.RelayOne = element.State.RelayOne;
            cabinet.RelayTwo = element.State.RelayTwo;
            cabinet.HumidityAlarm = element.State.HumidityAlarm;
            cabinet.TemperatureAlarm = element.State.TemperatureAlarm;
            cabinet.Dehumidify = element.State.Dehumidify;
            cabinet.ModifyTime = DateTime.Now;

            return true;
        }
    }
}
