// KeySetForm.cs
using System;
using System.Windows.Forms;

namespace Client3.WinForm
{
    public partial class KeySetForm : Form
    {
        private string KeyValue = string.Empty; // 키보드 값 받아서 저장 할 전역변수 
        private MacroSetForm msform = null; // MacroSetForm 객체를 받을 변수

        public KeySetForm(MacroSetForm form)
        {
            InitializeComponent();
            msform = form;
            this.KeyPreview = true; // 키값 받기 허용
            this.ActiveControl = KeySet_label; // label1에 포커싱
        }

        private void KeySetForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Ctrl 키와 다른 키가 함께 눌렸을 경우
            if (e.Control && e.KeyCode != Keys.ControlKey)
            {
                e.Handled = true; // 키 입력 이벤트 처리
                KeyValue = $"Ctrl + {e.KeyCode}";
                KeySet_label.Text = $"Ctrl + {e.KeyCode}";
            }
            // Shift 키와 다른 키가 함께 눌렸을 경우
            else if (e.Shift && e.KeyCode != Keys.ShiftKey)
            {
                e.Handled = true; // 키 입력 이벤트 처리
                KeyValue = $"Shift + {e.KeyCode}";
                KeySet_label.Text = $"Shift + {e.KeyCode}";
            }
            // Alt 키와 다른 키가 함께 눌렸을 경우
            else if (e.Alt && e.KeyCode != Keys.Menu)
            {
                e.Handled = true; // 키 입력 이벤트 처리
                KeyValue = $"Alt + {e.KeyCode}";
                KeySet_label.Text = $"Alt + {e.KeyCode}";
            }
            // Enter 키가 눌렸을 경우
            else if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true; // 키 입력 이벤트 처리
                KeyValue = "Enter";
                KeySet_label.Text = "Enter";
            }
            // 한/영 또는 한자 키가 눌렸을 경우
            else if (e.KeyCode == Keys.KanaMode || e.KeyCode == Keys.HanjaMode)
            {
                e.Handled = true; // 키 입력 이벤트 처리
            }
            // 그 외
            else
            {
                KeyValue = $"{e.KeyCode}";
                KeySet_label.Text = $"{e.KeyCode}";
            }
        }

        #region OK, Cancel Button
        // 확인 버튼 클릭 시
        private void OK_btn_Click(object sender, EventArgs e)
        {
            msform.tempKey = KeyValue; // MacroSetForm에 Key값 전달
            this.Close();
        }

        // 취소 버튼 클릭 시
        private void Cancel_btn_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }
        #endregion
    }
}
