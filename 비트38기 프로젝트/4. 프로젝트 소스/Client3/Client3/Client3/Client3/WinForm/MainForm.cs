//MainForm.cs
using Client3.WinForm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Client3
{
    public partial class MainForm : Form
    {
        private Control con = Control.Singleton;

        // 아이콘 설정
        private List<NotifyIcon> notifyIcons;
        // 아이콘 상태
        public int currentState;

        public MainForm()
        {
            InitializeComponent();

            InitializeNotifyIcons();

            Load();
        }


        #region Load, Closing
        private new async void Load()
        {
            // 토큰유무 확인
            if (File.Exists("token.dat"))
            {
                string protectedToken = File.ReadAllText("token.dat");
                var result = await con.auto(protectedToken);
                // 자동로그인 성공
                if (result.Item1 == true)
                {
                    string userid = result.Item2;
                    //VisibleChange(false, true, false, false);
                    currentState = 1;
                    UpdateNotifyIcon();
                    contextMenuStrip2.Items["아이디ToolStripMenuItem"].Enabled = false;
                    contextMenuStrip2.Items["아이디ToolStripMenuItem"].Text = userid;
                    con.GetInfo();
                    con.Load();
                }
                // 자동 로그인 실패
                else
                {
                    currentState = 2;
                    UpdateNotifyIcon();
                }
            }
            // 토큰파일 없음
            else
            {
                currentState = 2;
                UpdateNotifyIcon();
            }

        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            foreach (var notifyIcon in notifyIcons)
            {
                notifyIcon.Dispose();
            }
        }
        #endregion

        #region TrayIcon_Click
        private void 회원가입ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SignupForm newForm = new SignupForm();
            newForm.Show();
        }

        private void 로그인ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoginForm newForm = new LoginForm(this);
            newForm.Show();
        }

        private void 종료ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.notifyIcon1.Visible = false;
            con.Closing();
            Application.ExitThread();
        }
        #endregion

        #region form, TrayIcon 상태 변경



        // 시작해서 아이콘들 받아옴
        private void InitializeNotifyIcons()
        {
            notifyIcons = new List<NotifyIcon>
            {
                notifyIcon1,
                notifyIcon2,
                notifyIcon3
            };
        }

        // 현재 아이콘 상태들을 받아옴
        public void UpdateNotifyIcon()
        {
            for (int i = 0; i < notifyIcons.Count; i++)
            {
                notifyIcons[i].Visible = (currentState == i + 1);
                if (i == 2)
                {
                    notifyIcon2.ContextMenuStrip = contextMenuStrip1;
                }
            }
        }

        // 1아이콘일때 마우스 클릭
        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                currentState = 3;
                UpdateNotifyIcon();

                notifyIcon3.ContextMenuStrip = contextMenuStrip2;
                con.Closing();
            }
            else if(e.Button == MouseButtons.Right)
            {
                notifyIcon1.ContextMenuStrip = contextMenuStrip2;
            }
        }


        // 3아이콘일때 마우스 클릭
        private void notifyIcon3_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                currentState = 1;
                UpdateNotifyIcon();

                notifyIcon1.ContextMenuStrip = contextMenuStrip2;
                con.Load();
            }
            else if (e.Button == MouseButtons.Right)
            {
                notifyIcon3.ContextMenuStrip = contextMenuStrip2;
            }
        }

        #endregion

        #region 로그인시 TrayIcon_Click
        private void 로그아웃ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            con.Closing();
            MessageBox.Show("로그아웃 되었습니다.");

            currentState = 2;
            UpdateNotifyIcon();
        }

        private void 매크로설정ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            MacroSetForm msForm = new MacroSetForm();
            msForm.Show();
        }

        private void 회원탈퇴ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            WithdrawForm newForm = new WithdrawForm(this);
            newForm.Show();
        }

        private void 회원수정ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateForm newForm = new UpdateForm(this);
            newForm.Show();
        }

        private void 종료ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.notifyIcon1.Visible = false;
            con.Closing();
            Application.ExitThread();
        }


        #endregion


    }
}
