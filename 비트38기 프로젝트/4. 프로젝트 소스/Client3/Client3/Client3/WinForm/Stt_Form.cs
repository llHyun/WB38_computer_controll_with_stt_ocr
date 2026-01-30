// Stt_Form.cs
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace Client3.WinForm
{
    public partial class Stt_Form : Form
    {
        Timer timer = new Timer();
        int charIndex = 0;

        public Stt_Form()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.Manual;
            Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - Width) / 2, 0);
            label.AutoSize = true;
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.Text = "";

            timer.Interval = 200;
            timer.Tick += new EventHandler(timer1_Tick);
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            TopMost = true;
            Control con = Control.Singleton;

            if (charIndex < con.Stt_Result.Length)
            {
                label.Text += con.Stt_Result[charIndex];
                charIndex++;

                label.Left = (ClientSize.Width - label.Width) / 2;
                ClientSize = new Size(label.Width+1, 69);
                Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - Width) / 2, 0);
            }
            else
            {
                timer.Stop();
                charIndex = 0;
                Thread.Sleep(999);
                Close();
            }
        }

        private void Stt_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            label.Text = string.Empty;
        }

        private void Stt_Form_Load(object sender, EventArgs e)
        {
            timer.Start();
        }
    }
}
