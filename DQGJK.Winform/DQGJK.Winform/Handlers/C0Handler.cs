using DQGJK.Message;
using System;

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
