using Client3.Member;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Client3.WinForm
{
    public partial class MacroSetForm : Form
    {
        private Control con = Control.Singleton;

        private TabPage tabPage3 = null; // tagPage3의 존재유무를 구별하기 위한 변수

        public string tempKey = string.Empty; // KeySetForm에서 받은 Key값을 저장하기 위한 전역변수

        public MacroSetForm()
        {
            InitializeComponent();

            PrintAll();
        }

        #region tabPage1
        // Cmd_txt 키 입력 시 발생하는 이벤트
        private void Cmd_txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 한글 이외의 문자를 입력할 경우
            if (!(char.IsControl(e.KeyChar) || IsHangul(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void KeySet_Btn_Click(object sender, EventArgs e)
        {
            // KeySetForm을 호출하고 tempKey에 키 값을 받아옴
            KeySetForm keysetform = new KeySetForm(this);
            keysetform.ShowDialog();

            KeySet_btn.Text = tempKey;
        }

        private async void Save_btn_Click(object sender, EventArgs e)
        {
            // 공백 검사
            if (Cmd_txt.Text == string.Empty || KeySet_btn.Text == string.Empty)
            {
                MessageBox.Show("빈 칸이 있습니다.");
                return;
            }

            string cmdtxt = Cmd_txt.Text;
            string keyset = KeySet_btn.Text;

            Macro macro = new Macro(cmdtxt, keyset);

            // 중복 검사
            int idx = ValueToCmd(cmdtxt);
            if (idx != -1)
            {
                MessageBox.Show("중복되는 명령어입니다.");
                return;
            }

            idx = ValueToKey(keyset);
            if (idx != -1)
            {
                MessageBox.Show("중복되는 Key입니다.");
                return;
            }

            if (await con.InsertMacro(con.Id, macro.Cmd, macro.Key) == true)
            {
                MessageBox.Show("저장되었습니다.");

                Cmd_txt.Text = string.Empty;
                KeySet_btn.Text = string.Empty;

                PrintAll();
            }
            else
            {
                MessageBox.Show("저장 실패");

                Cmd_txt.Text = string.Empty;
                KeySet_btn.Text = string.Empty;
            }
        }
        #endregion

        #region tabPage2
        // 전체 출력
        private async void PrintAll()
        {
            dataGridView.Rows.Clear();
            con.mList.Clear();
            con.mList = await con.SelectAllToID(con.Id);

            Count_label.Text = $"저장개수 : {con.mList.Count}개";

            // dataGridView에 mList 삽입
            foreach (Macro m in con.mList)
            {
                dataGridView.Rows.Add(m.Cmd, m.Key);
            }
        }

        // dataGridView에서 셀 더블 클릭 시 발생하는 이벤트
        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // 헤더 또는 데이터가 없는 행을 클릭한 경우 이벤트를 종료
            if (e.RowIndex < 0 || dataGridView.RowCount == 0) { return; }

            // 더블 클릭 시 클릭한 행의 cmd와 key값을 가져옴
            string cmdValue = dataGridView.Rows[e.RowIndex].Cells["cmd"].Value.ToString();
            string keyValue = dataGridView.Rows[e.RowIndex].Cells["key"].Value.ToString();

            // tabPage3 탭 생성
            tabPage3 = new TabPage { Text = "명령어 수정" };

            #region tabPage3 라벨
            // '명령어 수정' 라벨 생성
            Label commandEditLabel = new Label
            {
                Text = "명령어 수정", // 라벨 텍스트
                Location = new Point(35, 69),  // 라벨 위치 설정
            };
            tabPage3.Controls.Add(commandEditLabel);  // tabPage3에 '명령어 수정' 라벨 추가

            // '키 수정' 라벨 생성
            Label keyEditLabel = new Label
            {
                Text = "키 수정", // 라벨 텍스트
                Location = new Point(273, 68),  // 라벨 위치 설정
            };
            tabPage3.Controls.Add(keyEditLabel);  // tabPage3에 '키 수정' 라벨 추가
            #endregion

            #region tabPage3 명령어값 입력 텍스트
            // TextBox 생성
            TextBox CmdUpdate_txt = new TextBox
            {
                Text = cmdValue,  // TextBox의 텍스트를 cmd값으로 설정
                Size = new Size(193, 35),  // TextBox 크기 설정
                Location = new Point(37, 119)  // TextBox 위치 설정
            };
            CmdUpdate_txt.Font = new Font(FontFamily.GenericSansSerif, 18);  // 폰트 크기를 18로 설정
            CmdUpdate_txt.KeyPress += (s, KeyPressEventArgs) =>
            { // 키 입력 이벤트 핸들러 추가
                // 한글 이외의 문자를 입력할 경우
                if (!(char.IsControl(KeyPressEventArgs.KeyChar) || IsHangul(KeyPressEventArgs.KeyChar)))
                {
                    KeyPressEventArgs.Handled = true;
                }
            };
            tabPage3.Controls.Add(CmdUpdate_txt);  // tabPage3에 TextBox 추가
            #endregion

            #region tabPage3 키값 입력 버튼
            // Button 생성
            Button KeyUpdate_btn = new Button
            {
                Text = keyValue,  // Button의 텍스트를 key값으로 설정
                Size = new Size(193, 35), // 버튼 크기 설정
                Location = new Point(275, 119),  // 버튼 위치 설정
                Font = new Font(FontFamily.GenericSansSerif, 18)  // 버튼의 폰트 크기를 18로 설정
            };
            // 키 수정 버튼 클릭 이벤트 핸들러 추가
            KeyUpdate_btn.Click += (s, eventArgs) => { KeySetForm keysetform = new KeySetForm(this); keysetform.ShowDialog(); KeyUpdate_btn.Text = tempKey; };
            tabPage3.Controls.Add(KeyUpdate_btn);  // tabPage3에 Button 추가
            #endregion

            #region tabPage3 수정 버튼
            // '수정' 버튼 생성
            Button editButton = new Button
            {
                Text = "수정",
                Location = new Point(335, 244),  // 버튼 위치 설정
                Size = new Size(76, 23)  // 버튼 크기 설정
            };
            // 키 수정 버튼 클릭 이벤트 핸들러 추가
            editButton.Click += async (s, eventArgs) =>
            {
                if (dataGridView.SelectedCells.Count > 0)
                {
                    int selectedIndex = dataGridView.SelectedCells[0].RowIndex; // 바꾼 것 없이 수정 버튼을 눌러도 수정하기 위해 자기 자신의 행을 가져옴

                    // 공백 검사
                    if (CmdUpdate_txt.Text == string.Empty || KeyUpdate_btn.Text == string.Empty)
                    {
                        MessageBox.Show("빈 칸이 있습니다.");
                        return;
                    }

                    string cmdtxt = CmdUpdate_txt.Text;
                    string keyset = KeyUpdate_btn.Text;

                    Macro macro = new Macro(cmdtxt, keyset);

                    // 중복 검사
                    int idx = ValueToCmd(cmdtxt);
                    if (idx != -1 && idx != selectedIndex) // 선택한 행 자신은 중복 검사에서 제외
                    {
                        MessageBox.Show("중복되는 명령어입니다.");
                        return;
                    }
                    idx = ValueToKey(keyset);
                    if (idx != -1 && idx != selectedIndex) // 선택한 행 자신은 중복 검사에서 제외
                    {
                        MessageBox.Show("중복되는 Key입니다.");
                        return;
                    }

                    if (cmdValue != cmdtxt || keyValue != keyset) // 처음 받아온 cmd값 또는 key값이 바뀌었을 경우
                    {
                        if (await con.UpdateMacro(con.Id, cmdValue, cmdtxt, keyset) == true)
                        {
                            MessageBox.Show("수정되었습니다.");
                            PrintAll();
                        }
                        else
                        {
                            MessageBox.Show("수정 실패");
                        }
                    }
                    else
                    {
                        MessageBox.Show("수정되었습니다.");
                        PrintAll();
                    }
                }

                tabControl1.TabPages.Remove(tabPage3); // tabPage3 삭제
                tabPage3 = null; // tabPage3 변수에 null값 부여(tabPage3가 존재하지 않는거로 판단)
                tabControl1.SelectedTab = tabPage2; // tabPage2로 이동
            };
            tabPage3.Controls.Add(editButton);  // tabPage3에 '수정' 버튼 추가
            #endregion

            #region tabPage3 취소 버튼
            // '취소' 버튼 생성
            Button cancelButton = new Button
            {
                Text = "취소",
                Location = new Point(427, 244),  // 버튼 위치 설정
                Size = new Size(76, 23)  // 버튼 크기 설정
            };
            cancelButton.Click += (s, eventArgs) => { tabControl1.TabPages.Remove(tabPage3); tabPage3 = null; tabControl1.SelectedTab = tabPage2; };  // 취소 버튼 클릭 이벤트 핸들러 추가
            tabPage3.Controls.Add(cancelButton);  // tabPage3에 '취소' 버튼 추가
            #endregion

            // TabControl에 새로운 TabPage 추가
            tabControl1.TabPages.Add(tabPage3);

            // 생성된 탭으로 포커스 이동
            tabControl1.SelectedTab = tabPage3;
        }

        private async void Delete_btn_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedCells.Count > 0)
            {
                string cmdValue = dataGridView.SelectedCells[1].Value.ToString();

                if (await con.DeleteMacro(con.Id, cmdValue) == true)
                {
                    MessageBox.Show("삭제되었습니다.");

                    PrintAll();
                }
                else
                {
                    MessageBox.Show("삭제 실패");
                }
            }
        }
        #endregion

        #region 내부 함수
        // 입력된 문자가 한글인지 판단
        bool IsHangul(char c)
        {
            // 한글 완성형 코드와 한글 자음/모음 코드를 모두 체크
            return (c >= 0xAC00 && c <= 0xD7A3) || (c >= 0x3131 && c <= 0x318E);
        }

        // cmd 찾기
        private int ValueToCmd(string cmd)
        {
            for (int i = 0; i < con.mList.Count; i++)
            {
                Macro macro = con.mList[i];
                if (macro.Cmd == cmd)
                    return i;  //찾은 인덱스
            }
            return -1;
        }

        // key 찾기
        private int ValueToKey(string key)
        {
            for (int i = 0; i < con.mList.Count; i++)
            {
                Macro macro = con.mList[i];
                if (macro.Key == key)
                    return i;  //찾은 인덱스
            }
            return -1;
        }

        // tabControl1 탭 페이지 변경 시 발생하는 이벤트
        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            // tabPage3가 null이 아닐 경우와 tabPage3가 선택된 상태에서 다른 탭 페이지로 이동을 시도할 경우
            if (tabPage3 != null && e.TabPage != tabPage3)
            {
                e.Cancel = true; // 이동을 막음
            }
        }
        #endregion
    }
}
