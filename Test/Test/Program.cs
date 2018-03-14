using DQGJK.Message;
using System;
using System.Collections.Generic;
using System.Net;
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
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(string.Format("发生错误：{0}", ex.Message));
                        }
                        break;
                    case "TOKENS":
                        try
                        {
                            if (listener == null) { Console.WriteLine("请先初始化TCP服务"); break; }

                            //string[] uids = listener.OnlineUID;

                            //Console.WriteLine(String.Join(",", uids));

                            List<AsyncUserTokenInfo> tokens = listener.OnlineUserToken;

                            foreach (var item in tokens)
                            {
                                Console.WriteLine("UID:" + item.UID);
                                Console.WriteLine("ConnectTime:" + item.ConnectTime);
                                Console.WriteLine("FreshTime:" + item.FreshTime);
                                Console.WriteLine("Address:" + item.Remote.Address);
                            }
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

        private static void Listener_OnMsgReceived(AsyncUserToken token, byte[] data)
        {
            Console.WriteLine("接收到数据：");
            Console.WriteLine(" 来源IP：" + token.Remote.Address.ToString());
            Console.WriteLine(" 连接时间：" + token.ConnectTime.ToString());
            Console.WriteLine(" 最近通讯时间：" + token.FreshTime.ToString());

            RecieveMessageDecode reader = new RecieveMessageDecode(data);
            RecieveMessage message = reader.Read();

            //string str = BytesUtil.ToHexString(message.ToByte());

            //Console.WriteLine(" 发送内容：" + str);
        }

        private static int Listener_GetPackageLength(byte[] data, out int headLength)
        {
            int length = MessageDecode.GetDataLength(data, out headLength);

            return length;
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
