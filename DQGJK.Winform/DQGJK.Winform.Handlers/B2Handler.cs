using DQGJK.Message;
using DQGJK.Winform.Models;
using System;

namespace DQGJK.Winform.Handlers
{
    internal class B2Handler : BaseHandler<B2Element>, IMessageHandler
    {
        public B2Handler(string UID, RecieveMessage Message)
        {
            _UID = UID;
            _Message = Message;
        }

        public RecieveMessage _Message { get; set; }

        public string _UID { get; set; }

        public void Handle()
        {
            MongoHandler.Save(new B2Data(_Message));

            UpdateCabinet(_Message);

            UpdateOperate(_Message, "B2");
        }

        public override bool SetCabinet(B2Element element, ref Cabinet cabinet)
        {
            cabinet.HumidityLimit = Convert.ToDecimal(element.HumidityLimit);
            cabinet.TemperatureLimit = Convert.ToDecimal(element.TemperatureLimit);
            return true;
        }
    }
}
