using DQGJK.Message;

namespace DQGJK.Winform.Handlers
{
    interface IMessageHandler
    {
        string _UID { get; set; }

        RecieveMessage _Message { get; set; }

        void Handle();
    }
}
