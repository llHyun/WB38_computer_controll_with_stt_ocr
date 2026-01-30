using System;
using System.Windows.Forms;


namespace Client3.WinForm
{
    public partial class UpdateForm : Form
    {
        private Control con = Control.Singleton;

        MainForm mainForm;
        public UpdateForm(MainForm mainForminterface)
        {
            InitializeComponent();
            mainForm = mainForminterface;
        }

        // 비번 암호화 on/off
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBox1.PasswordChar = default(char);
            }
            else
            {
                textBox1.PasswordChar = '*';
            }
        }

        // 회원 수정 버튼
        private async void button1_Click(object sender, EventArgs e)
        {
            string id = label1.Text;
            string pw = textBox1.Text;
            string name = textBox2.Text;
            string phone = textBox3.Text;

            string accinfo = string.Empty;

            accinfo += id + ",";
            accinfo += pw + ",";
            accinfo += name + ",";
            accinfo += phone;

            if (await con.Update(accinfo) == true)
            {
                this.Close();
                MessageBox.Show("회원수정 성공");
            }
            else{ MessageBox.Show("회원수정 실패"); }
        }

        // 폼 로드될때 contextmenu에 있는 id를 받아와서 서버에 보내줄때 사용함
        private void UpdateForm_Load(object sender, EventArgs e)
        {
            label1.Text = mainForm.contextMenuStrip2.Items["아이디ToolStripMenuItem"].Text;
        }
    }
}
