//LabelForm.cs
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Client3.WinForm
{
    internal class LabelForm
    {
        //투명한 폼 띄우기
        internal void Label_Form(List<Coordinate> text, Control con)
        {
            //폼 기본 설정->투명, 전체화면
            Form LabelForm = new Form
            {
                Text = "Label_Form",
                WindowState = FormWindowState.Maximized,
                BackColor = Color.Magenta,
                TransparencyKey = Color.Magenta,
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.Manual,
                Location = new Point(0, 0)
            };
            LabelForm.TopMost = true;
            LabelForm.Paint += (sender, e) =>
            {
                DrawCircleOnForm(text, LabelForm);
            };
            con.LabelForm = LabelForm;

            Application.Run(LabelForm);
        }

        //화면 위에 label 그리는 함수
        private void DrawCircleOnForm(List<Coordinate> text, Form form)
        {
            int i = 1;
            foreach (Coordinate cor in text)
            {
                //라벨 기본설정 -> 좌표값, 글꼴, 크기, 배경, 내용
                Label label = new Label
                {
                    Location = new Point(cor.LabelX, cor.LabelY),
                    Text = i.ToString(),
                    AutoSize = true,
                    Font = new Font("BinaryITC", 12),
                    ForeColor = Color.Black,
                    BackColor = Color.Red
                };
                form.Controls.Add(label);
                i++;
            }
        }
    }
}
