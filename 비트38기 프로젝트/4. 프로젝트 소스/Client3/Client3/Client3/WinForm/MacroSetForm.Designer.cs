namespace Client3.WinForm
{
    partial class MacroSetForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.Delete_btn = new System.Windows.Forms.Button();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.cmd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.key = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Count_label = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.Save_btn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.KeySet_btn = new System.Windows.Forms.Button();
            this.Cmd_txt = new System.Windows.Forms.TextBox();
            this.KeySet_cb = new System.Windows.Forms.ComboBox();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.Delete_btn);
            this.tabPage2.Controls.Add(this.dataGridView);
            this.tabPage2.Controls.Add(this.Count_label);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(511, 273);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "명령어 리스트";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // Delete_btn
            // 
            this.Delete_btn.Location = new System.Drawing.Point(453, 3);
            this.Delete_btn.Name = "Delete_btn";
            this.Delete_btn.Size = new System.Drawing.Size(55, 23);
            this.Delete_btn.TabIndex = 12;
            this.Delete_btn.Text = "삭제";
            this.Delete_btn.UseVisualStyleBackColor = true;
            this.Delete_btn.Click += new System.EventHandler(this.Delete_btn_Click);
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cmd,
            this.key});
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView.Location = new System.Drawing.Point(3, 27);
            this.dataGridView.MultiSelect = false;
            this.dataGridView.Name = "dataGridView";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridView.RowTemplate.Height = 23;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(505, 243);
            this.dataGridView.TabIndex = 7;
            this.dataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellDoubleClick);
            // 
            // cmd
            // 
            this.cmd.Frozen = true;
            this.cmd.HeaderText = "명령어";
            this.cmd.Name = "cmd";
            this.cmd.Width = 232;
            // 
            // key
            // 
            this.key.Frozen = true;
            this.key.HeaderText = "키";
            this.key.Name = "key";
            this.key.Width = 232;
            // 
            // Count_label
            // 
            this.Count_label.AutoSize = true;
            this.Count_label.Location = new System.Drawing.Point(3, 12);
            this.Count_label.Name = "Count_label";
            this.Count_label.Size = new System.Drawing.Size(88, 12);
            this.Count_label.TabIndex = 3;
            this.Count_label.Text = "저장 개수 : n개";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(519, 299);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.TabStop = false;
            this.tabControl1.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl1_Selecting);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.Save_btn);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.KeySet_btn);
            this.tabPage1.Controls.Add(this.Cmd_txt);
            this.tabPage1.Controls.Add(this.KeySet_cb);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(511, 273);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "명령어 입력";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // Save_btn
            // 
            this.Save_btn.Location = new System.Drawing.Point(427, 244);
            this.Save_btn.Name = "Save_btn";
            this.Save_btn.Size = new System.Drawing.Size(76, 23);
            this.Save_btn.TabIndex = 4;
            this.Save_btn.Text = "저장";
            this.Save_btn.UseVisualStyleBackColor = true;
            this.Save_btn.Click += new System.EventHandler(this.Save_btn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(273, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "키 입력";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "명령어 입력";
            // 
            // KeySet_btn
            // 
            this.KeySet_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeySet_btn.Location = new System.Drawing.Point(275, 119);
            this.KeySet_btn.Name = "KeySet_btn";
            this.KeySet_btn.Size = new System.Drawing.Size(193, 35);
            this.KeySet_btn.TabIndex = 1;
            this.KeySet_btn.UseVisualStyleBackColor = true;
            this.KeySet_btn.Click += new System.EventHandler(this.KeySet_Btn_Click);
            // 
            // Cmd_txt
            // 
            this.Cmd_txt.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cmd_txt.Location = new System.Drawing.Point(37, 119);
            this.Cmd_txt.Name = "Cmd_txt";
            this.Cmd_txt.Size = new System.Drawing.Size(193, 35);
            this.Cmd_txt.TabIndex = 0;
            this.Cmd_txt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Cmd_txt_KeyPress);
            // 
            // KeySet_cb
            // 
            this.KeySet_cb.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeySet_cb.FormattingEnabled = true;
            this.KeySet_cb.Items.AddRange(new object[] {
            "Alt + Tab",
            "LWin + D"});
            this.KeySet_cb.Location = new System.Drawing.Point(275, 119);
            this.KeySet_cb.Name = "KeySet_cb";
            this.KeySet_cb.Size = new System.Drawing.Size(207, 33);
            this.KeySet_cb.TabIndex = 6;
            this.KeySet_cb.SelectedIndexChanged += new System.EventHandler(this.KeySet_cb_SelectedIndexChanged_1);
            // 
            // MacroSetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 299);
            this.Controls.Add(this.tabControl1);
            this.Name = "MacroSetForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "명령어";
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridView;
        public System.Windows.Forms.Label Count_label;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmd;
        private System.Windows.Forms.DataGridViewTextBoxColumn key;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button Save_btn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Cmd_txt;
        public System.Windows.Forms.TabPage tabPage2;
        public System.Windows.Forms.Button KeySet_btn;
        private System.Windows.Forms.Button Delete_btn;
        private System.Windows.Forms.ComboBox KeySet_cb;
    }
}

