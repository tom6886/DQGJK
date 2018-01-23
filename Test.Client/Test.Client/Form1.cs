using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using TCPHandler;

namespace Test.Client
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        SocketClient smanager = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void btn_connnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(edit_ip.Text) || string.IsNullOrWhiteSpace(edit_port.Text)) { MessageBox.Show("请先填入IP和Port"); return; }

            if (Convert.ToInt16(edit_port.Text) <= 1000) { MessageBox.Show("端口号必须大于1000"); return; }

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(edit_ip.Text), Convert.ToInt16(edit_port.Text));

            SocketError res = Connect(endPoint);

            if (res == 0) { memoEdit1.MaskBox.AppendText("连接成功\r\n"); return; }

            memoEdit1.MaskBox.AppendText("连接失败，错误码：" + res);
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(memoEdit2.Text)) { MessageBox.Show("请输入发送内容"); return; }

            smanager.Send(ConvertMsg(memoEdit2.Text));
        }

        #region 方法

        private byte[] ConvertMsg(string text)
        {
            string[] strs = text.Split(' ');

            byte[] data = new byte[strs.Length];

            for (int i = 0, length = strs.Length; i < length; i++)
            {
                data[i] = Convert.ToByte(strs[i], 16);
            }

            return data;
        }

        public SocketError Connect(IPEndPoint endPoint)
        {
            if (smanager != null && smanager.Connected) return SocketError.Success;

            //创建连接对象, 连接到服务器  
            smanager = new SocketClient(endPoint);

            SocketError error = smanager.Connect();

            if (error == SocketError.Success)
            {
                //连接成功后,就注册事件. 最好在成功后再注册.  
                smanager.OnMsgReceived += Smanager_OnMsgReceived;
                smanager.GetPackageLength += Smanager_GetPackageLength;
                smanager.GetSendMessage += Smanager_GetSendMessage;
                smanager.OnSended += Smanager_OnSended;
            }
            return error;
        }

        private void Smanager_OnSended(bool successorfalse)
        {
            SetMemoText(successorfalse ? "发送消息成功\r\n" : "发送消息失败\r\n");
        }

        private byte[] Smanager_GetSendMessage(string msg)
        {
            if (smanager == null || !smanager.Connected) { MessageBox.Show("未连接到服务端，请重新连接"); return null; };

            byte[] sendBuffer = Encoding.UTF8.GetBytes(msg);

            byte[] buff = new byte[sendBuffer.Length + 4];
            Array.Copy(BitConverter.GetBytes(sendBuffer.Length), buff, 4);
            Array.Copy(sendBuffer, 0, buff, 4, sendBuffer.Length);

            return buff;
        }

        private int Smanager_GetPackageLength(byte[] data, out int headLength)
        {
            headLength = 4;
            byte[] lenBytes = new byte[4];
            Array.Copy(data, lenBytes, 4);
            int packageLen = BitConverter.ToInt32(lenBytes, 0);
            return packageLen;
        }

        private void Smanager_OnMsgReceived(byte[] info)
        {
            SetMemoText("接收到服务端的消息：" + Encoding.UTF8.GetString(info) + "\r\n");
        }

        private delegate void setMemoText(string str);

        private void SetMemoText(string str)
        {
            if (memoEdit1.InvokeRequired)
            {
                BeginInvoke(new setMemoText(SetMemoText), str);
            }
            else
            {
                memoEdit1.MaskBox.AppendText(str);
            }
        }
        #endregion
    }
}
