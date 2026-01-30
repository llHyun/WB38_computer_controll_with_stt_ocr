using System;
using System.Windows.Forms;

namespace Client3.WinForm
{
    public partial class WithdrawForm : Form
    {
        private Control con = Control.Singleton;

        MainForm mainForm;

        public WithdrawForm(MainForm mainForminterface)
        {
            InitializeComponent();
            mainForm = mainForminterface;
        }

        // 비번 암호 on/off
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

        // 회원 탈퇴 버튼
        private async void button1_Click(object sender, EventArgs e)
        {
            // 회원 탈퇴할때 다시한번 물어봐줌
            if (MessageBox.Show("회원탈퇴를 하시겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string id = textBox1.Text;
                string pw = textBox2.Text;
                string name = textBox3.Text;
                string phone = textBox4.Text;
                string email = textBox5.Text;

                string accinfo = string.Empty;

                accinfo += id + ",";
                accinfo += pw + ",";
                accinfo += name + ",";
                accinfo += phone + ",";
                accinfo += email;

                if (await con.Withdraw(accinfo) == true)
                {
                    this.Close();
                    MessageBox.Show("회원탈퇴 성공");
                }
                else
                {
                    MessageBox.Show("회원탈퇴 실패");
                }

            }
            else{}

        }
    }
}
