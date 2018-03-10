using DQGJK.Message;
using System;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using TCPHandler;

namespace DQGJK.Service
{
    public partial class Service1 : ServiceBase
    {
        internal static SocketListener listener;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            LogHelper.WriteLog("服务启动");
            StartListener();
        }

        protected override void OnStop()
        {
            LogHelper.WriteLog("服务停止");
            StopListener();
        }

        private void StartListener()
        {
            try
            {
                //监听的端口号
                int port = Convert.ToInt16(ConfigurationManager.AppSettings["port"]);
                //最大可连接数
                int connect = Convert.ToInt16(ConfigurationManager.AppSettings["connect"]);
                //缓冲区大小
                int buffer = Convert.ToInt16(ConfigurationManager.AppSettings["buffer"]);

                listener = new SocketListener(connect, buffer);

                listener.Init();

                listener.Start(new IPEndPoint(IPAddress.Any, port));

                listener.OnClientNumberChange += Listener_OnClientNumberChange;

                listener.GetIDByEndPoint += Listener_GetIDByEndPoint;

                listener.GetPackageLength += Listener_GetPackageLength;

                listener.OnMsgReceived += Listener_OnMsgReceived;

                listener.OnSended += Listener_OnSended; ;

                LogHelper.WriteLog("启动成功，开始监听端口 " + port + "\r\n");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("启动监听失败", ex.Message, ex.StackTrace);
            }
        }

        private void StopListener()
        {
            try
            {
                int port = Convert.ToInt16(ConfigurationManager.AppSettings["port"]);

                listener.Stop();

                LogHelper.WriteLog("关闭成功，停止监听端口 " + port + "\r\n");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("关闭监听时出错", ex.Message, ex.StackTrace);
            }
        }

        private void Listener_OnSended(AsyncUserToken token, SocketError error)
        {

        }

        private void Listener_OnMsgReceived(AsyncUserToken token, byte[] info)
        {
            string str = BytesUtil.ToHexString(info);

            try
            {
                RecieveMessageDecode reader = new RecieveMessageDecode(info);
                RecieveMessage message = reader.Read();
                MessageHandler msgHandler = new MessageHandler(token.UID, message);
                msgHandler.OnIPChanged += MsgHandler_OnIPChanged;
                msgHandler.OnMsgSend += MsgHandler_OnMsgSend;
                msgHandler.Set();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("接收消息时出错", "接收到的消息：" + str + "\r\n" + ex.Message, ex.StackTrace);
            }
        }

        private void MsgHandler_OnMsgSend(string UID, byte[] msg)
        {
            try
            {
                listener.Send(UID, msg);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("发送消息时出错", ex.Message, ex.StackTrace);
            }
        }

        private void MsgHandler_OnIPChanged(string UID)
        {
            try
            {
                listener.CloseClientSocket(UID);
            }
            catch (Exception)
            {
                LogHelper.WriteLog("关闭连接时出错");
            }
        }

        private int Listener_GetPackageLength(byte[] data, out int headLength)
        {
            try
            {
                return MessageDecode.GetDataLength(data, out headLength);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("计算消息长度时出错", ex.Message, ex.StackTrace);
                headLength = -1;
                return 43;
            }
        }

        private string Listener_GetIDByEndPoint(IPEndPoint endPoint)
        {
            if (endPoint == null) { return null; }

            return endPoint.GetHashCode().ToString();
        }

        private void Listener_OnClientNumberChange(int number, AsyncUserToken token)
        {

        }
    }
}
