namespace Client3
{
    partial class RecordForm
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.next_btn = new System.Windows.Forms.Button();
            this.exit_btn = new System.Windows.Forms.Button();
            this.pic1 = new System.Windows.Forms.PictureBox();
            this.pic2 = new System.Windows.Forms.PictureBox();
            this.pic3 = new System.Windows.Forms.PictureBox();
            this.pic4 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pic1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic4)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F);
            this.label2.ForeColor = System.Drawing.Color.Gray;
            this.label2.Location = new System.Drawing.Point(224, 357);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 29);
            this.label2.TabIndex = 4;
            this.label2.Text = "label2";
            // 
            // next_btn
            // 
            this.next_btn.Location = new System.Drawing.Point(421, 25);
            this.next_btn.Name = "next_btn";
            this.next_btn.Size = new System.Drawing.Size(89, 28);
            this.next_btn.TabIndex = 6;
            this.next_btn.Text = "다음 녹음";
            this.next_btn.UseVisualStyleBackColor = true;
            this.next_btn.Click += new System.EventHandler(this.next_btn_Click);
            // 
            // exit_btn
            // 
            this.exit_btn.Location = new System.Drawing.Point(421, 25);
            this.exit_btn.Name = "exit_btn";
            this.exit_btn.Size = new System.Drawing.Size(89, 28);
            this.exit_btn.TabIndex = 7;
            this.exit_btn.Text = "닫기";
            this.exit_btn.UseVisualStyleBackColor = true;
            this.exit_btn.Click += new System.EventHandler(this.exit_btn_Click);
            // 
            // pic1
            // 
            this.pic1.Image = global::Client3.Properties.Resources.Mic_Off;
            this.pic1.Location = new System.Drawing.Point(1, 0);
            this.pic1.Name = "pic1";
            this.pic1.Size = new System.Drawing.Size(530, 450);
            this.pic1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic1.TabIndex = 3;
            this.pic1.TabStop = false;
            // 
            // pic2
            // 
            this.pic2.Image = global::Client3.Properties.Resources.Mic_On;
            this.pic2.Location = new System.Drawing.Point(1, 0);
            this.pic2.Name = "pic2";
            this.pic2.Size = new System.Drawing.Size(530, 450);
            this.pic2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic2.TabIndex = 5;
            this.pic2.TabStop = false;
            // 
            // pic3
            // 
            this.pic3.Image = global::Client3.Properties.Resources.MICONGIF1;
            this.pic3.Location = new System.Drawing.Point(1, 0);
            this.pic3.Name = "pic3";
            this.pic3.Size = new System.Drawing.Size(530, 450);
            this.pic3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic3.TabIndex = 1;
            this.pic3.TabStop = false;
            // 
            // pic4
            // 
            this.pic4.Image = global::Client3.Properties.Resources.Check;
            this.pic4.Location = new System.Drawing.Point(1, 0);
            this.pic4.Name = "pic4";
            this.pic4.Size = new System.Drawing.Size(530, 450);
            this.pic4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic4.TabIndex = 0;
            this.pic4.TabStop = false;
            // 
            // RecordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(531, 451);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pic1);
            this.Controls.Add(this.pic2);
            this.Controls.Add(this.pic3);
            this.Controls.Add(this.exit_btn);
            this.Controls.Add(this.next_btn);
            this.Controls.Add(this.pic4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "RecordForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "음성 녹음";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RecordForm_FormClosing);
            this.Load += new System.EventHandler(this.RecordForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pic1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox pic4;
        private System.Windows.Forms.PictureBox pic3;
        private System.Windows.Forms.PictureBox pic2;
        public System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button next_btn;
        public System.Windows.Forms.PictureBox pic1;
        private System.Windows.Forms.Button exit_btn;
    }
}