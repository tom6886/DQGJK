using DQGJK.Message;
using System;
using System.Windows.Forms;

namespace DQGJK.Winform
{
    public partial class Commond : Form
    {
        public delegate void SetCommondHandler(string uid, byte[] msg);

        public event SetCommondHandler SetCommond;

        private byte[] currentMsg;

        private string currentUID;

        public Commond()
        {
            InitializeComponent();
        }

        private void btn_create_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cb_type.Text)) { MessageBox.Show("请选择指令类型"); return; }

            if (string.IsNullOrEmpty(tb_device.Text)) { MessageBox.Show("请填入遥测站地址"); return; }

            object uid = CacheUtil.GetCache(tb_device.Text);

            if (uid == null) { MessageBox.Show("未找到在线的遥测站，请确认遥测站地址"); return; }

            if (!cb_type.Text.Equals("B0") && string.IsNullOrEmpty(edit_content.Text)) { MessageBox.Show("请输入修改内容"); return; }

            SendMessage msg = new SendMessage();
            msg.CenterCode = 0x01;
            msg.ClientCode = BytesUtil.ToHexArray(tb_device.Text);
            msg.SendTime = DateTime.Now;
            msg.Serial = 0;
            msg.FunctionCode = cb_type.Text;

            if (!cb_type.Text.Equals("B0"))
            {
                msg.Body = BytesUtil.ToHexArray(edit_content.Text);
            }

            memoEdit1.Text = BytesUtil.ToHexString(msg.ToByte());

            currentMsg = msg.ToByte();

            currentUID = uid.ToString();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            SetCommond?.Invoke(currentUID, currentMsg);

            this.Close();
        }
    }
}
