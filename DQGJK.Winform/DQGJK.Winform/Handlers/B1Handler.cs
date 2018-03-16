﻿using DQGJK.Message;

namespace DQGJK.Winform.Handlers
{
    internal class B1Handler : BaseHandler<B1Element>, IMessageHandler
    {
        public B1Handler(string UID, RecieveMessage Message)
        {
            _UID = UID;
            _Message = Message;
        }

        public RecieveMessage _Message { get; set; }

        public string _UID { get; set; }

        public void Handle()
        {
            MongoHandler.Save(new B1Data(_Message));

            UpdateCabinet(_Message);
        }

        public override bool SetCabinet(B1Element element, ref Cabinet cabinet)
        {
            cabinet.RelayOne = element.State.RelayOne;
            cabinet.RelayTwo = element.State.RelayTwo;
            cabinet.Dehumidify = element.State.Dehumidify;
            return true;
        }
    }
}
