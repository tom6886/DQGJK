namespace DQGJK.Winform
{
    partial class Main1
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.deviceGrid = new DevExpress.XtraGrid.GridControl();
            this.DeviceGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deviceGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1184, 761);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1176, 728);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "基础信息";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.deviceGrid);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1176, 728);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "设备列表";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // deviceGrid
            // 
            this.deviceGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.deviceGrid.Location = new System.Drawing.Point(3, 3);
            this.deviceGrid.MainView = this.DeviceGridView;
            this.deviceGrid.Name = "deviceGrid";
            this.deviceGrid.Size = new System.Drawing.Size(1170, 722);
            this.deviceGrid.TabIndex = 1;
            this.deviceGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.DeviceGridView});
            // 
            // DeviceGridView
            // 
            this.DeviceGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4});
            this.DeviceGridView.GridControl = this.deviceGrid;
            this.DeviceGridView.Name = "DeviceGridView";
            this.DeviceGridView.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "遥测站地址";
            this.gridColumn1.FieldName = "ClientCode";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 1;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "遥测站IP";
            this.gridColumn2.FieldName = "ClientIP";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 0;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "上线时间";
            this.gridColumn3.FieldName = "InTime";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 3;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "最近通讯时间";
            this.gridColumn4.FieldName = "ModifyTime";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 2;
            // 
            // Main1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 761);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Main1";
            this.ShowIcon = false;
            this.Text = "物联网城市电网柜环境巡检系统上位机程序";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main1_FormClosing);
            this.Load += new System.EventHandler(this.Main1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.deviceGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private DevExpress.XtraGrid.GridControl deviceGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView DeviceGridView;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
    }
}