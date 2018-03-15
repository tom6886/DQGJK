using DQGJK.Message;

namespace DQGJK.Winform.Handlers
{
    internal class B2Handler : IMessageHandler
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
        }
    }
}
