using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TCPHandler;

namespace DQGJK.Winform
{
    public partial class Devices : Form
    {
        public delegate void OnSelectHandle(string code);

        public event OnSelectHandle OnSelect;

        public Devices()
        {
            InitializeComponent();
        }

        private void Devices_Load(object sender, EventArgs e)
        {
            var task = Task.Factory.StartNew(() => { BindTable(); });
        }

        private void BindTable()
        {
            if (Main.listener == null) { return; }

            List<AsyncUserTokenInfo> tokens = Main.listener.OnlineUserToken;

            ConcurrentDictionary<string, string> online = Main.online;

            List<TableRow> list = new List<TableRow>();

            foreach (var item in online)
            {
                TableRow row = new TableRow();
                row.ClientCode = item.Key;
                AsyncUserTokenInfo info = tokens.Where(q => q.UID.Equals(item.Value)).FirstOrDefault();
                if (info != null)
                {
                    row.ClientIP = info.Remote.Address.ToString();
                    row.InTime = info.ConnectTime;
                    row.ModifyTime = info.FreshTime;
                }
                list.Add(row);
            }

            BindData(list);
        }

        private class TableRow
        {
            public string ClientCode { get; set; }

            public string ClientIP { get; set; }

            public DateTime InTime { get; set; }

            public DateTime ModifyTime { get; set; }
        }

        private delegate void bindData(List<TableRow> list);

        private void BindData(List<TableRow> list)
        {
            if (dataGridView1.InvokeRequired)
            {
                BeginInvoke(new bindData(BindData), list);
            }
            else
            {
                dataGridView1.DataSource = list;
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            OnSelect?.Invoke(dataGridView1.Rows[e.RowIndex].Cells["ClientCode"].Value.ToString());
            this.Close();
        }
    }
}
