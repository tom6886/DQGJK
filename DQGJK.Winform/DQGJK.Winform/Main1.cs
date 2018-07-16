using DevExpress.XtraGrid;
using DQGJK.Message;
using DQGJK.Winform.Handlers;
using DQGJK.Winform.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using TCPHandler;
using XUtils;

namespace DQGJK.Winform
{
    public partial class Main1 : Form
    {
        internal static SocketListener _listener;
        internal static ConcurrentDictionary<string, string> _online;
        internal static BindingList<DeviceRow> _devices;

        #region 初始化
        private static Main1 frm = null;

        private Main1(SocketListener listener)
        {
            _listener = listener;

            _listener.OnClientNumberChange += Listener_OnClientNumberChange;

            _listener.GetIDByEndPoint += Listener_GetIDByEndPoint;

            _listener.GetPackageLength += Listener_GetPackageLength;

            _listener.OnMsgReceived += Listener_OnMsgReceived;

            _listener.OnSended += Listener_OnSended;

            _online = new ConcurrentDictionary<string, string>();

            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
        }

        public static Main1 CreateInstrance(SocketListener listener)
        {
            if (frm == null)
            {
                frm = new Main1(listener);
            }
            return frm;
        }

        private void Main1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void Main1_Load(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() => { BindDeviceGrid(); });
        }
        #endregion

        #region 设备列表
        private void BindDeviceGrid()
        {
            _devices = new BindingList<DeviceRow>();
            BindGrid(deviceGrid, _devices);
        }


        #endregion

        #region TCPHandler接口处理
        private void Listener_OnSended(AsyncUserToken token, SocketError error)
        {
            try
            {
                //StringBuilder sb = new StringBuilder();

                //if (error != SocketError.Success)
                //{
                //    sb.Append("发送消息时出错：" + error);
                //    sb.Append("\r\n");
                //    AppendLog(sb.ToString());
                //    return;
                //}

                //sb.Append("已发送消息：");
                //sb.Append("\r\n");
                //sb.Append(" 发送IP：" + token.Remote.Address.ToString());
                //sb.Append("\r\n");
                //sb.Append(" 发送时间：" + DateTime.Now);
                //sb.Append("\r\n");
                //AppendLog(sb.ToString());
            }
            catch (Exception ex)
            {
                //LogHelper.WriteLog("发送消息时出错", ex.Message, ex.StackTrace);
            }
        }

        private void Listener_OnMsgReceived(AsyncUserToken token, byte[] info)
        {
            string str = BytesUtil.ToHexString(info);

            try
            {
                RecieveMessageDecode reader = new RecieveMessageDecode(info);
                RecieveMessage message = reader.Read();

                //StringBuilder sb = new StringBuilder();
                //sb.Append("接收到数据：");
                //sb.Append("\r\n");
                //sb.Append(" 来源IP：" + token.Remote.Address.ToString());
                //sb.Append("\r\n");
                //sb.Append(" 接收时间：" + DateTime.Now);
                //sb.Append("\r\n");
                //sb.Append(" 数据类型：" + message.FunctionCode);
                //sb.Append("\r\n");
                //sb.Append(" 接收内容：" + str);
                //sb.Append("\r\n");
                //AppendLog(sb.ToString());

                //更新设备缓存
                UpdateCache(message.ClientCodeStr, token.UID);
                if (message.FunctionCode.Equals("F2")) { return; }
                IMessageHandler handler = HandlerFactory.Create(message.FunctionCode, token.UID, message);
                handler.Handle();

                //如果是设备自报数据，向设备发送接收成功的报文
                if (message.FunctionCode.Equals("C0"))
                {
                    SendMessage res = new SendMessage();
                    res.ClientCode = message.ClientCode;
                    res.CenterCode = message.CenterCode;
                    res.SendTime = DateTime.Now;
                    res.Serial = 0;
                    res.FunctionCode = "C0";

                    _listener.Send(token.UID, res.ToByte());
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("接收消息时出错", "接收到的消息：" + str + "\r\n" + ex.Message, ex.StackTrace);
            }
        }

        private void UpdateCache(string client, string uid)
        {
            string _uid;

            _online.TryGetValue(client, out _uid);

            if (_uid != null && _uid.Equals(uid)) { return; }

            if (_uid == null)
            {
                _online.TryAdd(client, uid);
                //新增设备列表记录
                AsyncUserTokenInfo info = _listener.OnlineUserToken.Where(q => q.UID.Equals(uid)).FirstOrDefault();
                if (info != null)
                {
                    AddDevice(new DeviceRow()
                    {
                        ClientCode = client,
                        ClientIP = info.Remote.Address.ToString(),
                        InTime = info.ConnectTime.ToString(),
                        ModifyTime = info.FreshTime.ToString()
                    });
                }
                //BindGrid(deviceGrid, _devices);
                //数据库更新上线时间
                ConnectionHelper.OnLine(client);
            }
            else
            {
                _online.TryUpdate(client, uid, _uid);
                //如果IP地址变化，则关闭之前的连接
                _listener.CloseClientSocket(_uid);
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
                //LogHelper.WriteLog("获取包长时出错", ex.Message, ex.StackTrace);
                headLength = 0;
                return 43;
            }
        }

        private string Listener_GetIDByEndPoint(IPEndPoint endPoint)
        {
            try
            {
                if (endPoint == null) { return null; }

                return endPoint.GetHashCode().ToString();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("获取UID时出错", ex.Message, ex.StackTrace);
                return StringUtil.UniqueID();
            }
        }

        private void Listener_OnClientNumberChange(int number, AsyncUserToken token)
        {
            try
            {
                //StringBuilder sb = new StringBuilder();
                //sb.Append(number > 0 ? "有设备接入" : "有设备断开");
                //sb.Append("\r\n");
                //sb.Append(" 来源IP：" + token.Remote.Address.ToString());
                //sb.Append("\r\n");
                //sb.Append(" 连接时间：" + token.ConnectTime.ToString());
                //sb.Append("\r\n");
                //AppendLog(sb.ToString());

                string uid = token.UID;

                if (number > 0 || string.IsNullOrEmpty(uid)) { return; }

                KeyValuePair<string, string> item = _online.Where(q => q.Value.Equals(uid)).FirstOrDefault();

                if (default(KeyValuePair<string, string>).Equals(item)) { return; }

                ConnectionHelper.OffLine(item.Key);

                _online.TryRemove(item.Key, out uid);
            }
            catch (Exception ex)
            {
                //LogHelper.WriteLog("设备接入/断开时出错", ex.Message, ex.StackTrace);
            }
        }
        #endregion

        #region UI操作
        private delegate void bindGrid<T>(GridControl grid, BindingList<T> list);

        private void BindGrid<T>(GridControl grid, BindingList<T> list)
        {
            if (grid.InvokeRequired)
            {
                BeginInvoke(new bindGrid<T>(BindGrid), grid, list);
            }
            else
            {
                grid.DataSource = list;
            }
        }

        private delegate void addDevice(DeviceRow device);

        private void AddDevice(DeviceRow device)
        {
            if (deviceGrid.InvokeRequired)
            {
                BeginInvoke(new addDevice(AddDevice), device);
            }
            else
            {
                _devices.Add(device);
            }
        }
        #endregion
    }
}
