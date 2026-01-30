using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client3.WinForm
{
    public partial class progress : Form
    {

        public progress()
        {
            InitializeComponent();
        }

        private async Task ShowProgressBarAndWaitAsync()
        {
            // 폼을 가운데로 표시
            CenterToScreen();

            // 프로그레스 바를 10초 동안 채우기
            for (int i = 0; i < 10; i++)
            {
                if (i == 0)
                {
                    label1.Text = "사용자마다 모델이 달라요!";
                }
                else if (i == 3)
                {
                    label1.Text = "모델 깨우는중";
                }
                else if(i == 6)
                {
                    label1.Text = "모델 옷입히는중";
                }
                else if (i == 9)
                {
                    label1.Text = "모델 준비 완료 :>";
                }
                await Task.Delay(1000);  // 1초 대기
                progressBar1.PerformStep();
            }
        }

        private async void progress_Load(object sender, EventArgs e)
        {
            await ShowProgressBarAndWaitAsync();
            progressBar1.Value = 0;
            Close();  // 작업이 끝나면 폼을 닫음
            
        }
    }
}
