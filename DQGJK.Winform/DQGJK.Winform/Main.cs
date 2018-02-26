using DQGJK.Message;
using System;
using System.Net;
using System.Text;
using System.Windows.Forms;
using TCPHandler;

namespace DQGJK.Winform
{
    public partial class Main : Form
    {
        static SocketListener listener;

        public Main()
        {
            InitializeComponent();
        }

        private void btn1_Click(object sender, System.EventArgs e)
        {
            int tag = Convert.ToInt16(btn1.Tag);

            if (tag == 0) { StartListener(); }
            else { StopListener(); }
        }

        private void StartListener()
        {
            try
            {
                int port = Convert.ToInt16(tb_port.Text);

                int connect = Convert.ToInt16(tb_connect.Text);

                int buffer = Convert.ToInt16(tb_buffer.Text);

                listener = new SocketListener(connect, buffer);

                listener.Init();

                listener.Start(new IPEndPoint(IPAddress.Parse("127.0.0.1"), port));

                listener.OnClientNumberChange += Listener_OnClientNumberChange;

                listener.GetIDByEndPoint += Listener_GetIDByEndPoint;

                listener.GetPackageLength += Listener_GetPackageLength;

                listener.OnMsgReceived += Listener_OnMsgReceived;

                listener.OnSended += Listener_OnSended;

                btn1.Text = "停止";

                btn1.Tag = 1;

                AppendLog("启动成功，开始监听端口 " + port + "\r\n");
            }
            catch (Exception ex)
            {
                AppendLog(ex.Message);
            }
        }

        private void StopListener()
        {
            try
            {
                int port = Convert.ToInt16(tb_port.Text);

                listener.Stop();

                btn1.Text = "启动";

                btn1.Tag = 0;

                AppendLog("关闭成功，停止监听端口 " + port + "\r\n");
            }
            catch (Exception ex)
            {
                AppendLog(ex.Message);
            }
        }

        private void Listener_OnSended(AsyncUserToken token, System.Net.Sockets.SocketError error)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("已发送消息：");
            sb.Append("\r\n");
            sb.Append(" 发送IP：" + token.Remote.Address.ToString());
            sb.Append("\r\n");
            AppendLog(sb.ToString());
        }

        private void Listener_OnMsgReceived(AsyncUserToken token, byte[] info)
        {
            MessageDecode reader = new MessageDecode(info);
            RecieveMessage message = reader.Read();
            string str = BytesUtil.ToHexString(info);

            StringBuilder sb = new StringBuilder();
            sb.Append("接收到数据：");
            sb.Append("\r\n");
            sb.Append(" 来源IP：" + token.Remote.Address.ToString());
            sb.Append("\r\n");
            sb.Append(" 发送内容：" + str);
            sb.Append("\r\n");
            AppendLog(sb.ToString());

            SendMessage res = new SendMessage();
            res.ClientCode = message.ClientCode;
            res.CenterCode = message.CenterCode;
            res.SendTime = message.SendTime;
            res.Serial = message.Serial;
            //res.SendTime = DateTime.Now;
            //res.Serial = message.Serial + 1;
            res.FunctionCode = "C0";

            listener.Send(token.UID, res.ToByte());

        }

        private int Listener_GetPackageLength(byte[] data, out int headLength)
        {
            return MessageDecode.GetDataLength(data, out headLength);
        }

        private string Listener_GetIDByEndPoint(IPEndPoint endPoint)
        {
            if (endPoint == null) { return null; }

            return endPoint.GetHashCode().ToString();
        }

        private void Listener_OnClientNumberChange(int number, AsyncUserToken token)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(number > 0 ? "有设备接入" : "有设备断开");
            sb.Append("\r\n");
            sb.Append(" 来源IP：" + token.Remote.Address.ToString());
            sb.Append("\r\n");
            sb.Append(" 连接时间：" + token.ConnectTime.ToString());
            sb.Append("\r\n");
            AppendLog(sb.ToString());
        }

        private delegate void appendLog(string log);

        private void AppendLog(string log)
        {
            if (edit_log.InvokeRequired)
            {
                BeginInvoke(new appendLog(AppendLog), log);
            }
            else
            {
                edit_log.MaskBox.AppendText(log);
            }
        }
    }
}
