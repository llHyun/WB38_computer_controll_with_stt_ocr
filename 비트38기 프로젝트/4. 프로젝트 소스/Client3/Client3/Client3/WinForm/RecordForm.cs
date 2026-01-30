// RecordForm.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Client3
{
    public partial class RecordForm : Form
    {
        Control con = Control.Singleton;
        FileSystemWatcher watcher = new FileSystemWatcher();
        Timer timer = new Timer();
        string folder = @"C:\Temp\client\study_voice";
        public List<string> Sentence = new List<string>();
        int count = 0;
        int charIndex = 0;
        int y = 1;

        public RecordForm()
        {

            InitializeComponent();

            #region 문장입력란
            Sentence.Add("\"가장 인기있는 음악 선택\", 을 말해주세요");
            Sentence.Add("\"어디로 가야하나요? \", 를 말해주세요");
            Sentence.Add("\"저 집에 가고싶어요\", 를 말해주세요");
            Sentence.Add("\"내 말이 그말이야\", 를 말해주세요");
            Sentence.Add("\"가장 빠른 길 선택해줘\", 을 말해주세요");
            #endregion

            exit_btn.Visible = false;

            label2.AutoSize = true;
            label2.TextAlign = ContentAlignment.MiddleLeft;
            label2.Text = "";

            watcher.Path = folder;
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName; //파일 생성 및 덮어쓰기 감지

            watcher.Changed += OnFileChanged;

            watcher.EnableRaisingEvents = true;
        }

        public void start_again()
        {
            timer.Interval = 100;
            timer.Tick += new EventHandler(timer1_Tick);
            timer.Start();  // 타이머를 시작합니다.
            pic1_On();
        }

        private void DeleteStudyRecord()
        {
            foreach (var file in Directory.GetFiles(folder))
            {
                File.Delete(file);
            }
        }

        private void RecordForm_Load(object sender, EventArgs e)
        {
            DeleteStudyRecord();
            start_again();
            con.RecordForm = this;
        }

        private void RecordForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // 지정된 디렉토리에서 .wav 파일을 가져오기
                var wavFiles = Directory.EnumerateFiles(folder, "*.wav").ToList();

                // .wav 파일이 정확히 10개가 아니라면 모든 .wav 파일을 삭제
                if (wavFiles.Count != 5)
                {
                    foreach (var filePath in wavFiles)
                    {
                        File.Delete(filePath);
                    }
                }
                timer.Tick -= timer1_Tick;
                timer.Stop();
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
                con.study_RecordEnd();  //이거 이미 종료됐으면 오류남
            }
            catch (Exception)
            {

            }
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            if (charIndex < Sentence[count].Length)
            {
                label2.Text += Sentence[count][charIndex];
                charIndex++;

                label2.Left = (this.ClientSize.Width - label2.Width) / 2;
            }
            else
            {
                timer.Tick -= timer1_Tick;
                timer.Stop();
                con.study_RecordStart();
                pic2_On();
                charIndex = 0;
                count ++;
            }
        }

        public void pic1_On()
        {
            // PictureBox3와 Label2를 숨기고 PictureBox2와 Label1을 표시
            pic4.Visible = false;

            pic1.Visible = true;
            label2.Visible = true;
        }

        public void pic2_On()
        {
            // PictureBox3와 Label2를 숨기고 PictureBox2와 Label1을 표시
            pic1.Visible = false;

            pic2.Visible = true;
        }

        public void pic3_On()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(pic3_On));
                return;
            }
            pic2.Visible = false;

            pic3.Visible = true;
        }

        public void pic4_On()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(pic4_On));
                return;
            }
            label2.Text = string.Empty;
            label2.Visible = false;
            pic3.Visible = false;
            pic4.Visible = true;
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            if (y == 1)
            {
                pic3_On();
                y++;
            }
            else if (y == 2)
            {
                y++;
            }
            else if (y == 3)
            {
                pic4_On();
                y = 1;

                // 지정된 디렉토리에서 .wav 파일을 가져오기
                var wavFiles = Directory.EnumerateFiles(folder, "*.wav").ToList();

                if (wavFiles.Count == 5)
                {
                    if (this.IsHandleCreated)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            next_btn.Visible = false;
                            exit_btn.Visible = true;
                        });
                    }
                }
            }
        }

        private void next_btn_Click(object sender, EventArgs e)
        {
            start_again();
        }

        private void exit_btn_Click(object sender, EventArgs e)
        {
            con.study_RecordEnd();
            this.Close();
        }
    }
}
