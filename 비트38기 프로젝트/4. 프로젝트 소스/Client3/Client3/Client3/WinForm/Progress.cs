// Progress.cs
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client3.WinForm
{
    public partial class Progress : Form
    {

        public Progress()
        {
            InitializeComponent();
        }

        private async Task ShowProgressBarAndWaitAsync()
        {
            // 폼을 가운데로 표시
            CenterToScreen();

            // 프로그레스 바를 10초 동안 채우기
            for (int i = 0; i < 30; i++)
            {
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
