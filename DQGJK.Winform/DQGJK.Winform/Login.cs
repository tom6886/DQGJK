using System;
using System.Net;
using System.Windows.Forms;
using TCPHandler;

namespace DQGJK.Winform
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            try
            {
                int port = Convert.ToInt16(te_port.Text);

                int connect = Convert.ToInt16(te_connect.Text);

                int buffer = Convert.ToInt16(te_buffer.Text);

                SocketListener listener = new SocketListener(connect, buffer);

                listener.Init();

                listener.Start(new IPEndPoint(IPAddress.Any, port));

                Main1 main = Main1.CreateInstrance(listener);

                main.Show();

                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("连接错误：" + ex.Message);
            }
        }

        private void StartListener()
        {
            try
            {
                int port = Convert.ToInt16(te_port.Text);

                int connect = Convert.ToInt16(te_connect.Text);

                int buffer = Convert.ToInt16(te_buffer.Text);

                SocketListener listener = new SocketListener(connect, buffer);

                listener.Init();

                listener.Start(new IPEndPoint(IPAddress.Any, port));

                //listener.OnClientNumberChange += Listener_OnClientNumberChange;

                //listener.GetIDByEndPoint += Listener_GetIDByEndPoint;

                //listener.GetPackageLength += Listener_GetPackageLength;

                //listener.OnMsgReceived += Listener_OnMsgReceived;

                //listener.OnSended += Listener_OnSended;

                //online = new ConcurrentDictionary<string, string>();

                //btn1.Text = "停止";

                //btn1.Tag = 1;

                //AppendLog("启动成功，开始监听端口 " + port + "\r\n");

                Main1 main = Main1.CreateInstrance(listener);

                main.Show();

                this.Hide();
            }
            catch (Exception ex)
            {
                //AppendLog(ex.Message);
                MessageBox.Show("连接错误：" + ex.Message);
            }
        }
    }
}
