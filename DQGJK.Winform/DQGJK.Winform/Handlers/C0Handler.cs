using DQGJK.Message;
using System;

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

            Response();

            UpdateCabinet(_Message);
        }

        public override bool SetCabinet(B0C0Element element, ref Cabinet cabinet)
        {
            if (!element.Valid) { return false; }

            cabinet.Humidity = Convert.ToDecimal(element.Humidity);
            cabinet.Temperature = Convert.ToDecimal(element.Temperature);
            cabinet.HumidityLimit = Convert.ToDecimal(element.HumidityLimit);
            cabinet.TemperatureLimit = Convert.ToDecimal(element.TemperatureLimit);
            cabinet.RelayOne = element.State.RelayOne;
            cabinet.RelayTwo = element.State.RelayTwo;
            cabinet.HumidityAlarm = element.State.HumidityAlarm;
            cabinet.TemperatureAlarm = element.State.TemperatureAlarm;
            cabinet.Dehumidify = element.State.Dehumidify;

            return true;
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
    }
}
