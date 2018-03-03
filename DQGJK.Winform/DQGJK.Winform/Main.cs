using DQGJK.Message;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using TCPHandler;

namespace DQGJK.Winform
{
    public partial class Main : Form
    {
        internal static SocketListener listener;

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

                listener.Start(new IPEndPoint(IPAddress.Any, port));

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

        private void Listener_OnSended(AsyncUserToken token, SocketError error)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("已发送消息：");
            sb.Append("\r\n");
            sb.Append(" 发送IP：" + token.Remote.Address.ToString());
            sb.Append("\r\n");
            sb.Append(" 发送时间：" + DateTime.Now);
            sb.Append("\r\n");
            AppendLog(sb.ToString());
        }

        private void Listener_OnMsgReceived(AsyncUserToken token, byte[] info)
        {
            string str = BytesUtil.ToHexString(info);

            try
            {
                RecieveMessageDecode reader = new RecieveMessageDecode(info);
                RecieveMessage message = reader.Read();

                StringBuilder sb = new StringBuilder();
                sb.Append("接收到数据：");
                sb.Append("\r\n");
                sb.Append(" 来源IP：" + token.Remote.Address.ToString());
                sb.Append("\r\n");
                sb.Append(" 接收时间：" + DateTime.Now);
                sb.Append("\r\n");
                sb.Append(" 接收内容：" + str);
                sb.Append("\r\n");
                AppendLog(sb.ToString());

                MessageHandler.Set(new MessageToken() { UID = token.UID, Message = message });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("接收消息时出错", "接收到的消息：" + str + "\r\n" + ex.Message, ex.StackTrace);
            }
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

        private void btn_commond_Click(object sender, EventArgs e)
        {
            Commond form = new Commond();
            form.StartPosition = FormStartPosition.CenterParent;
            form.SetCommond += Form_SetCommond;
            form.ShowDialog(this);
        }

        private void Form_SetCommond(string uid, byte[] msg)
        {
            listener.Send(uid, msg);
        }
    }
}
