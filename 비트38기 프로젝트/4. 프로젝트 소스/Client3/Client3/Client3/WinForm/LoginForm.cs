//LoginForm.cs
using System;
using System.Windows.Forms;

namespace Client3
{
    public partial class LoginForm : Form
    {
        private Control con = Control.Singleton;

        MainForm mainForm;
        public LoginForm(MainForm mainForminterface)
        {
            InitializeComponent();
            mainForm = mainForminterface;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string id = textBox1.Text;
            string pw = textBox2.Text;
            string set = string.Empty;

            set += id + ",";
            set += pw;

            var result = await con.Login(set);
            // 로그인 성공시
            if (result.Item1 == true)
            {
                this.Close();
                MessageBox.Show("로그인 성공");
                string userid = result.Item2; //사용자 id 받아오기
                //form에다가 띄워주기
                con.Statenum = 3;
                mainForm.UpdateNotifyIcon();
                mainForm.contextMenuStrip2.Items["아이디ToolStripMenuItem"].Enabled = false;
                mainForm.contextMenuStrip2.Items["아이디ToolStripMenuItem"].Text = userid;
                con.GetInfo();
            }
            //로그인 실패시
            else
            {
                this.Close();
                MessageBox.Show("로그인 실패");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked == true)
            {
                textBox2.PasswordChar = default(char);
            }
            else
            {
                textBox2.PasswordChar = '*';
            }
        }
    }
}
