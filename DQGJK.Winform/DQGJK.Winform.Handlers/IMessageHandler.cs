using DQGJK.Message;

namespace DQGJK.Winform.Handlers
{
    public interface IMessageHandler
    {
        string _UID { get; set; }

        RecieveMessage _Message { get; set; }

        void Handle();
    }
}
