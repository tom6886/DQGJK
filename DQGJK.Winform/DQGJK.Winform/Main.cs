﻿using DQGJK.Message;
using DQGJK.Winform.Handlers;
using DQGJK.Winform.Helpers;
using DQGJK.Winform.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using TCPHandler;
using XUtils;

namespace DQGJK.Winform
{
    public partial class Main : Form
    {
        internal static SocketListener listener;

        internal static ConcurrentDictionary<string, string> online;

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

                online = new ConcurrentDictionary<string, string>();

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

                online = new ConcurrentDictionary<string, string>();

                btn1.Text = "启动";

                btn1.Tag = 0;

                AppendLog("关闭成功，停止监听端口 " + port + "\r\n");
            }
            catch (Exception ex)
            {
                AppendLog(ex.Message);
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

        private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                List<AsyncUserTokenInfo> tokens = listener.OnlineUserToken;

                DateTime limit = DateTime.Now - new TimeSpan(0, 5, 0);

                List<AsyncUserTokenInfo> overtime = tokens.Where(q => q.FreshTime < limit).ToList();

                if (overtime.Count == 0) { return; }

                StringBuilder sb = new StringBuilder();
                sb.Append(string.Format("检测到 {0} 个连接超时，准备进行主动断开", overtime.Count));
                sb.Append("\r\n");

                AppendLog(sb.ToString());

                foreach (var item in overtime)
                {
                    listener.CloseClientSocket(item.UID);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("主动断开连接时出错", ex.Message, ex.StackTrace);
            }
        }

        #region TCPHandler接口处理
        private void Listener_OnSended(AsyncUserToken token, SocketError error)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                if (error != SocketError.Success)
                {
                    sb.Append("发送消息时出错：" + error);
                    sb.Append("\r\n");
                    AppendLog(sb.ToString());
                    return;
                }

                sb.Append("已发送消息：");
                sb.Append("\r\n");
                sb.Append(" 发送IP：" + token.Remote.Address.ToString());
                sb.Append("\r\n");
                sb.Append(" 发送时间：" + DateTime.Now);
                sb.Append("\r\n");
                AppendLog(sb.ToString());
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("发送消息时出错", ex.Message, ex.StackTrace);
            }
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
                sb.Append(" 数据类型：" + message.FunctionCode);
                sb.Append("\r\n");
                sb.Append(" 接收内容：" + str);
                sb.Append("\r\n");
                AppendLog(sb.ToString());

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

                    listener.Send(token.UID, res.ToByte());
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

            online.TryGetValue(client, out _uid);

            if (_uid != null && _uid.Equals(uid)) { return; }

            if (_uid == null)
            {
                online.TryAdd(client, uid);
                ConnectionHelper.OnLine(client);
            }
            else
            {
                online.TryUpdate(client, uid, _uid);
                //如果IP地址变化，则关闭之前的连接
                listener.CloseClientSocket(_uid);
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
                LogHelper.WriteLog("获取包长时出错", ex.Message, ex.StackTrace);
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
                StringBuilder sb = new StringBuilder();
                sb.Append(number > 0 ? "有设备接入" : "有设备断开");
                sb.Append("\r\n");
                sb.Append(" 来源IP：" + token.Remote.Address.ToString());
                sb.Append("\r\n");
                sb.Append(" 连接时间：" + token.ConnectTime.ToString());
                sb.Append("\r\n");
                AppendLog(sb.ToString());

                string uid = token.UID;

                if (number > 0 || string.IsNullOrEmpty(uid)) { return; }

                KeyValuePair<string, string> item = online.Where(q => q.Value.Equals(uid)).FirstOrDefault();

                if (default(KeyValuePair<string, string>).Equals(item)) { return; }

                ConnectionHelper.OffLine(item.Key);

                online.TryRemove(item.Key, out uid);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("设备接入/断开时出错", ex.Message, ex.StackTrace);
            }
        }
        #endregion

        #region 操作UI
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
        #endregion

        private void timer2_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (edit_log.Text.Count() > 10000)
            {
                edit_log.Text = "";
            }

            try
            {
                using (DBContext db = new DBContext())
                {
                    DateTime now = DateTime.Now;

                    //获取未发送过的消息
                    List<Operate> list = db.Operate.Where(q => q.State == OperateState.Before).ToList();

                    //获取发送过但超过五分钟没有返回报文的消息
                    DateTime board = now - new TimeSpan(0, 5, 0);
                    List<Operate> sended = db.Operate.Where(q => q.State == OperateState.Sended && q.RetryCount < 6 && q.SendTime < board).ToList();

                    list.AddRange(sended);

                    if (list.Count == 0) { return; }

                    List<string> ids = list.Select(q => q.ID).ToList();

                    List<DeviceOperate> subList = db.DeviceOperate.Where(q => ids.Contains(q.OperateID)).ToList();

                    list = list.OrderBy(q => q.CreateTime).ToList();

                    foreach (var item in list)
                    {
                        db.Entry(MessageHelper.OperateHandle(item, subList.Where(q => q.OperateID.Equals(item.ID)).ToList(), now)).State = EntityState.Modified;
                    }

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("下发命令时出错", ex.Message, ex.StackTrace);
            }
        }

        private void brn_stat_Click(object sender, EventArgs e)
        {
            Statistic form = new Statistic();
            form.ShowDialog(this);
        }

        private void timer3_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                MongoHelper.StatMongoData(DateTime.Now.AddDays(-1));
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("统计数据时出错", ex.Message, ex.StackTrace);
            }
        }
    }
}