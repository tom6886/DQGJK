using DQGJK.Message;
using System.Reflection;

namespace DQGJK.Winform.Handlers
{
    public class HandlerFactory
    {
        public static IMessageHandler Create(string FunctionCode, string UID, RecieveMessage Message)
        {
            IHandlerFactory factory = (IHandlerFactory)Assembly.Load("DQGJK.Winform.Handlers").CreateInstance("DQGJK.Winform.Handlers." + FunctionCode + "Factory");
            return factory.CreateHandler(UID, Message);
        }
    }

    interface IHandlerFactory
    {
        IMessageHandler CreateHandler(string UID, RecieveMessage Message);
    }

    public class B0Factory : IHandlerFactory
    {
        public IMessageHandler CreateHandler(string UID, RecieveMessage Message)
        {
            return new B0Handler(UID, Message);
        }
    }

    public class B1Factory : IHandlerFactory
    {
        public IMessageHandler CreateHandler(string UID, RecieveMessage Message)
        {
            return new B1Handler(UID, Message);
        }
    }

    public class B2Factory : IHandlerFactory
    {
        public IMessageHandler CreateHandler(string UID, RecieveMessage Message)
        {
            return new B2Handler(UID, Message);
        }
    }

    public class B3Factory : IHandlerFactory
    {
        public IMessageHandler CreateHandler(string UID, RecieveMessage Message)
        {
            return new B3Handler(UID, Message);
        }
    }

    public class C0Factory : IHandlerFactory
    {
        public IMessageHandler CreateHandler(string UID, RecieveMessage Message)
        {
            return new C0Handler(UID, Message);
        }
    }
}
