// Program.cs
using System;
using System.Threading;
using System.Windows.Forms;

namespace Client3
{
    internal static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 원래 있던 클라가 있으면 실행 안되게 하기
            Mutex mutex = new Mutex(false, "Client3.exe");

            if (!mutex.WaitOne(0, false))
            {
                MessageBox.Show("프로그램이 이미 실행 중입니다.", "중복 실행 오류", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm form = new MainForm();
            Application.Run();

            // 실행되는 동안 mutex가 가비지 컬렉션에 의해 수집되지 않도록 하기
            GC.KeepAlive(mutex);
        }
    }
}
