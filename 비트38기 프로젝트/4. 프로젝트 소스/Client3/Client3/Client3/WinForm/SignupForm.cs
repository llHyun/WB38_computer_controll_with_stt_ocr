//SignupForm.cs
using Client3.WinForm;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client3
{
    public partial class SignupForm : Form
    {
        private Control con = Control.Singleton;
        private string path = "C:\\Temp\\client\\study_voice";

        public SignupForm()
        {
            InitializeComponent();
        }

        private async void signin_btn_Click(object sender, EventArgs e)
        {
            string id = textBox1.Text;
            string pw = textBox2.Text;
            string name = textBox3.Text;
            string phone = textBox4.Text;
            string email = textBox5.Text;

            string accinfo = string.Empty;

            // 정규 표현식 패턴(비밀번호 조건)
            string pattern = @"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[\W]).{8,}$";
            var wavFiles = Directory.EnumerateFiles(path, "*.wav").ToList();

            //비번에 조건 있는지 확인
            if (Regex.IsMatch(pw, pattern))
            {
                accinfo += id + ",";
                accinfo += pw + ",";
                accinfo += name + ",";
                accinfo += phone + ",";
                accinfo += email;

                if (wavFiles.Count == 5)
                {
                    if (await con.signup(accinfo) == true)
                    {
                        Progress progress = new Progress();
                        this.Hide();

                        // 값을 받으면 경고가 안떠요
                        var labelStack = Task.Run(() => progress.ShowDialog());
                        try
                        {
                            // 비동기 함수 호출 및 결과 대기
                            await con.SendStudyVoice(id, path);

                            // 비동기 함수가 완료되면 UI를 갱신
                            this.Show();
                        }
                        catch (Exception)
                        {
                        }
                        finally
                        {
                            // Progress 닫기
                            progress.Close();
                        }
                        this.Close();
                        MessageBox.Show("모델 학습 완료");
                    }
                    else
                    {
                        MessageBox.Show("회원가입 실패");
                    }
                }
                else
                {
                    MessageBox.Show("음성녹음파일이 없습니다. 녹음해주세요");
                }
                
            }
            else
            {
                MessageBox.Show("비밀번호가 적합하지 않습니다.");
                textBox2.Text = string.Empty;
            }
        }

        private void record_btn_Click(object sender, EventArgs e)
        {
            RecordForm recordForm = new RecordForm();
            recordForm.ShowDialog();
        }

        #region 내부 함수
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
        #endregion
    }
}
