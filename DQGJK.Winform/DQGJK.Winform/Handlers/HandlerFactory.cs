using DQGJK.Message;
using System.Reflection;

namespace DQGJK.Winform.Handlers
{
    internal class HandlerFactory
    {
        public static IMessageHandler Create(string FunctionCode, string UID, RecieveMessage Message)
        {
            IHandlerFactory factory = (IHandlerFactory)Assembly.Load("DQGJK.Winform").CreateInstance("DQGJK.Winform.Handlers." + FunctionCode + "Factory");
            return factory.CreateHandler(UID, Message);
        }
    }

    interface IHandlerFactory
    {
        IMessageHandler CreateHandler(string UID, RecieveMessage Message);
    }

    internal class B0Factory : IHandlerFactory
    {
        public IMessageHandler CreateHandler(string UID, RecieveMessage Message)
        {
            return new B0Handler(UID, Message);
        }
    }

    internal class B1Factory : IHandlerFactory
    {
        public IMessageHandler CreateHandler(string UID, RecieveMessage Message)
        {
            return new B1Handler(UID, Message);
        }
    }

    internal class B2Factory : IHandlerFactory
    {
        public IMessageHandler CreateHandler(string UID, RecieveMessage Message)
        {
            return new B2Handler(UID, Message);
        }
    }

    internal class B3Factory : IHandlerFactory
    {
        public IMessageHandler CreateHandler(string UID, RecieveMessage Message)
        {
            return new B3Handler(UID, Message);
        }
    }

    internal class C0Factory : IHandlerFactory
    {
        public IMessageHandler CreateHandler(string UID, RecieveMessage Message)
        {
            return new C0Handler(UID, Message);
        }
    }
}
