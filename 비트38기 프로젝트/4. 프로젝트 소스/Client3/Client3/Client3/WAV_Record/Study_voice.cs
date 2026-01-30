// Study_voice.cs
using System;
using System.Diagnostics;

namespace Client3.WAV_Record
{
    internal class Study_voice
    {
        //파이썬 코드 호출 -> 실시간 음성 들으며 데시벨 일정 수준 넘으면 파일로 저장
        public void study_RecordStart()
        {
            Process process = new Process();
            process.StartInfo.FileName = "python";
            process.StartInfo.Arguments = "studyvoice.py";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            if (process.Start())
            {
                Control con = Control.Singleton;

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

                study_RecordEnd();
            }
        }

        public void study_RecordEnd()
        {
            Control con = Control.Singleton;
            if (con.studyProcess != null && !con.studyProcess.HasExited)
            {
                con.studyProcess.Kill();
                con.studyProcess = null;
            }
        }
    }
}
