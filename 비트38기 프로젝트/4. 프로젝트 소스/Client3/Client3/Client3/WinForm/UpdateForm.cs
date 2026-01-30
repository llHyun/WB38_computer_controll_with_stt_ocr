// UpdateForm.cs
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Client3.WinForm
{
    public partial class UpdateForm : Form
    {
        private Control con = Control.Singleton;
        private MainForm mainForm;

        public UpdateForm(MainForm mainForminterface)
        {
            InitializeComponent();
            mainForm = mainForminterface;
        }

        // 회원 수정 버튼
        private async void button1_Click(object sender, EventArgs e)
        {
            string id = label1.Text;
            string old_pw = textBox1.Text;
            string new_pw = textBox2.Text;
            string check_new_pw = textBox3.Text;

            if (new_pw == check_new_pw)
            {
                string pattern = @"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[\W]).{8,}$";

                //비번에 조건 있는지 확인
                if (Regex.IsMatch(new_pw, pattern))
                {
                    string accinfo = string.Empty;

                    accinfo += id + ",";
                    accinfo += old_pw + ",";
                    accinfo += new_pw;

                    if (await con.Update(accinfo) == true)
                    {
                        this.Close();
                        MessageBox.Show("회원수정 성공");
                    }
                    else { MessageBox.Show("회원수정 실패"); }
                }
                else
                {
                    MessageBox.Show("비밀번호는 영문, 숫자, 특수문자포함 8자 이상이어야됩니다.");
                    textBox2.Text = string.Empty;
                }
            }
            else
            {
                MessageBox.Show("비밀번호가 일치하지 않습니다.");
                textBox2.Text = string.Empty;
                textBox3.Text = string.Empty;
            }
        }

        // 폼 로드될때 contextmenu에 있는 id를 받아와서 서버에 보내줄때 사용함
        private void UpdateForm_Load(object sender, EventArgs e)
        {
            label1.Text = mainForm.contextMenuStrip2.Items["아이디ToolStripMenuItem"].Text;
        }
    }
}
