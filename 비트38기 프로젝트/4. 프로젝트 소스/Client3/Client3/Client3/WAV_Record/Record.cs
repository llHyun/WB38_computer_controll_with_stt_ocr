// Record.cs
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Client3.WAV_Record
{
    internal class Record
    {
        static bool StopDouble = false;
        const string ScreenshotPath = @"C:\Temp\client\screenshot.png";
        const string audioFilePath = @"C:\Temp\client\record\lastrec.wav";
        string lockFilePath = Path.Combine(Directory.GetCurrentDirectory(), "python_script.lock");

        //파이썬 코드 호출 -> 실시간 음성 들으며 데시벨 일정 수준 넘으면 파일로 저장
        public void RecordStart()
        {
            Control con = Control.Singleton;

            string folder = "C:\\Temp\\client\\record";
            bool fileExists = File.Exists(lockFilePath);

            if (fileExists)
            {
                return;
            }

            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = folder;
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName; //파일 생성 및 덮어쓰기 감지

            watcher.Changed += OnFileChanged;

            watcher.EnableRaisingEvents = true;

            Process process = new Process();
            process.StartInfo.FileName = "python";
            process.StartInfo.Arguments = "the_king_.py";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            if(process.Start())
            {
                try
                {
                    //결과값을 비동기로 받아오는 코드
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                }
                catch (InvalidOperationException)
                {

                }
                con.recProcess = process;
                process.WaitForExit();
                
                // 중복 실행을 막는 LOCK 파일 삭제하는 코드
                try
                {
                    File.Delete(lockFilePath);
                }
                catch (Exception)
                {

                }

                //디렉토리 변경 감지 종료
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
            }

            
        }

        public void RecordEnd()
        {
            Control con = Control.Singleton;

            if(con.recProcess!= null)
            {
                con.recProcess.Kill();
                con.recProcess = null;

                try
                {
                    File.Delete(lockFilePath);
                }
                catch (Exception)
                {

                }
            }
        }
       
        private async void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            Control control = Control.Singleton;

            //덮어쓰기 이전에 파일 새로 생성, 덮어쓰기로 이루어지기때문에
            //2번실행되어서 한번만 실행되도록 수정
            if (StopDouble)
            {
                //라벨이 띄워져있는지 확인하고 상황에 맞게 서버에게 전송
                if (control.IsLabel)
                {
                    StopDouble = false;
                    RecordEnd();
                    await SendTTN();
                }
                else
                {
                    StopDouble = false;
                    RecordEnd();
                    await SendWAV();
                }
            }
            else
            {
                StopDouble = true;
            }

        }

        private async Task SendWAV()
        {
            Control control = Control.Singleton;
            control.ScreenShot();

            using (var fs = new FileStream(ScreenshotPath, FileMode.Open, FileAccess.Read))
            {
                var ms = new MemoryStream();
                fs.CopyTo(ms);
                ms.Position = 0;
                await control.SendWAV(control.Id, Convert.ToBase64String(ms.ToArray()), audioFilePath);
            };
        }
        private async Task SendTTN()
        {
            Control control = Control.Singleton;
            await control.SendTTN(control.Id, audioFilePath);
        }
    }
}
