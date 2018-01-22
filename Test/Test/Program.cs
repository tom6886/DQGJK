using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TCPHandler;

namespace Test
{
    class Program
    {
        static SocketListener listener;

        static void Main(string[] args)
        {
            string com = "";
            do
            {
                com = Console.ReadLine().ToUpper();

                switch (com)
                {
                    case "TCP":
                        try
                        {
                            listener = new SocketListener(200, 1024);

                            listener.Init();

                            listener.Start(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 13909));

                            listener.OnClientNumberChange += Listener_OnClientNumberChange;

                            listener.GetIDByEndPoint += Listener_GetIDByEndPoint;

                            listener.GetPackageLength += Listener_GetPackageLength;

                            listener.OnMsgReceived += Listener_OnMsgReceived;

                            listener.GetSendMessage += Listener_GetSendMessage;

                            listener.OnSended += Listener_OnSended;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(string.Format("发生错误：{0}", ex.Message));
                        }
                        break;
                    case "UID":
                        try
                        {
                            if (listener == null) { Console.WriteLine("请先初始化TCP服务"); break; }

                            string[] uids = listener.OnlineUID;

                            Console.WriteLine(String.Join(",", uids));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(string.Format("发生错误：{0}", ex.Message));
                        }

                        break;
                    default:
                        Console.WriteLine("无法识别的命令");
                        break;
                }

            } while (com.ToUpper() != "EXIT");
        }

        private static int Listener_GetPackageLength(byte[] data, out int headLength)
        {
            throw new NotImplementedException();
        }

        private static void Listener_OnClientNumberChange(int number, AsyncUserToken token)
        {
            Console.WriteLine(number > 0 ? "有设备接入" : "有设备断开");
            Console.WriteLine(" 来源IP：" + token.Remote.Address.ToString());
            Console.WriteLine(" 连接时间：" + token.ConnectTime.ToString());
        }

        private static string Listener_GetIDByEndPoint(IPEndPoint endPoint)
        {
            return endPoint.GetHashCode().ToString();
        }
    }
}
