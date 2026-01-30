// GuideForm.cs
using System;
using System.Windows.Forms;

namespace Client3.WinForm
{
    public partial class GuideForm : Form
    {
        int i = 0;

        public GuideForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(i == 0)
            {
                pictureBox3.BringToFront(); i++;
            }
            else if(i == 1)
            {
                pictureBox4.BringToFront(); i++;
            }
            else if (i == 2)
            {
                pictureBox5.BringToFront(); i++;
            }
            else if(i == 3)
            {
                pictureBox2.BringToFront();
                i = 0;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (i == 0)
            {
                pictureBox5.BringToFront(); i = 3;
            }
            else if (i == 1)
            {
                pictureBox2.BringToFront(); i--;
            }
            else if (i == 2)
            {
                pictureBox3.BringToFront(); i--;
            }
            else if (i == 3)
            {
                pictureBox4.BringToFront(); i--;
            }
        }
    }
}
