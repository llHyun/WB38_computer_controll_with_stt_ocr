using System;
using System.Diagnostics;
using System.IO;


namespace Client3.WAV_Record
{
    internal class Study_voice
    {
        static bool StopDouble = false;
        //파이썬 코드 호출 -> 실시간 음성 들으며 데시벨 일정 수준 넘으면 파일로 저장
        public void study_RecordStart()
        {
            Control con = Control.Singleton;
            string folder = "C:\\Temp\\client\\study_voice";

            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = folder;
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName; //파일 생성 및 덮어쓰기 감지

            watcher.Changed += OnFileChanged;

            watcher.EnableRaisingEvents = true;

            Process process = new Process();
            process.StartInfo.FileName = "python";
            process.StartInfo.Arguments = "studyvoice.py";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            if (process.Start())
            {
                try
                {
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                }
                catch (InvalidOperationException)
                {

                }
                con.studyProcess = process;
                process.WaitForExit();
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
            }


        }
        public void study_RecordEnd()
        {
            Control con = Control.Singleton;
            if (con.studyProcess != null)
            {
                con.studyProcess.Kill();
                con.studyProcess = null;
            }
            //녹음된거 10개 아니면 다 지우는 코드 넣기
        }


        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            Control control = Control.Singleton;
            //덮어쓰기 이전에 파일 새로 생성, 덮어쓰기로 이루어지기때문에
            //2번실행될지 3번실행될지 모름 로직 수정할 것
            if (StopDouble)
            {
               
            }
            else
            {
                StopDouble = true;
            }
        }
    }
}
