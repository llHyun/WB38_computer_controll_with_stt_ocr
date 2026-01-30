//SignupForm.cs
using Client3.WinForm;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Client3
{
    public partial class SignupForm : Form
    {
        private Control con = Control.Singleton;

        public SignupForm()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string id = textBox1.Text;
            string pw = textBox2.Text;
            string name = textBox3.Text;
            string phone = textBox4.Text;
            string email = textBox5.Text;

            string accinfo = string.Empty;

            // 정규 표현식 패턴(비밀번호 조건)
            string pattern = @"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[\W]).{8,}$";
            //비번에 조건 있는지 확인
            if (Regex.IsMatch(pw, pattern))
            {
                accinfo += id + ",";
                accinfo += pw + ",";
                accinfo += name + ",";
                accinfo += phone + ",";
                accinfo += email;
                if (await con.signup(accinfo) == true)
                {
                    this.Close();
                    MessageBox.Show("회원가입 성공");

                }
                else
                {
                    MessageBox.Show("회원가입 실패");
                }
            }
            else
            {
                MessageBox.Show("비밀번호가 적합하지 않습니다.");
                textBox2.Text = string.Empty;
            }

            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
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
