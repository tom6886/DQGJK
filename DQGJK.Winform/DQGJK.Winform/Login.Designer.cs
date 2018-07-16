namespace DQGJK.Winform
{
    partial class Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.te_port = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.te_connect = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.te_buffer = new DevExpress.XtraEditors.TextEdit();
            this.btn_login = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.te_port.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.te_connect.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.te_buffer.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // te_port
            // 
            this.te_port.EditValue = "8899";
            this.te_port.Location = new System.Drawing.Point(129, 10);
            this.te_port.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.te_port.Name = "te_port";
            this.te_port.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.te_port.Properties.Appearance.Options.UseFont = true;
            this.te_port.Size = new System.Drawing.Size(169, 34);
            this.te_port.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelControl1.Location = new System.Drawing.Point(33, 13);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(90, 28);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "监听端口:";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelControl2.Location = new System.Drawing.Point(12, 49);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(111, 28);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "最大连接数:";
            // 
            // te_connect
            // 
            this.te_connect.EditValue = "200";
            this.te_connect.Location = new System.Drawing.Point(129, 46);
            this.te_connect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.te_connect.Name = "te_connect";
            this.te_connect.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.te_connect.Properties.Appearance.Options.UseFont = true;
            this.te_connect.Size = new System.Drawing.Size(169, 34);
            this.te_connect.TabIndex = 2;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelControl3.Location = new System.Drawing.Point(12, 85);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(111, 28);
            this.labelControl3.TabIndex = 5;
            this.labelControl3.Text = "消息缓冲区:";
            // 
            // te_buffer
            // 
            this.te_buffer.EditValue = "1024";
            this.te_buffer.Location = new System.Drawing.Point(129, 82);
            this.te_buffer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.te_buffer.Name = "te_buffer";
            this.te_buffer.Properties.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.te_buffer.Properties.Appearance.Options.UseFont = true;
            this.te_buffer.Size = new System.Drawing.Size(169, 34);
            this.te_buffer.TabIndex = 4;
            // 
            // btn_login
            // 
            this.btn_login.Appearance.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_login.Appearance.Options.UseFont = true;
            this.btn_login.Location = new System.Drawing.Point(304, 12);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(99, 101);
            this.btn_login.TabIndex = 6;
            this.btn_login.Text = "连接";
            this.btn_login.Click += new System.EventHandler(this.btn_login_Click);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(413, 126);
            this.Controls.Add(this.btn_login);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.te_buffer);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.te_connect);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.te_port);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "Login";
            this.ShowIcon = false;
            this.Text = "IP/端口号";
            ((System.ComponentModel.ISupportInitialize)(this.te_port.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.te_connect.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.te_buffer.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit te_port;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit te_connect;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit te_buffer;
        private DevExpress.XtraEditors.SimpleButton btn_login;
    }
}