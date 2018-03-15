using DQGJK.Message;

namespace DQGJK.Winform.Handlers
{
    internal class B1Handler : IMessageHandler
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
        }
    }
}
