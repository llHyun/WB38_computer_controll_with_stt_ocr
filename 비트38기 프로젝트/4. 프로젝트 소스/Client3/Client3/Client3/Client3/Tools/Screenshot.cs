//Screenshot.cs
using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Client3
{
    public class Screenshot
    {
        public void Capture()
        {
            try
            {
                // 주화면의 크기 정보 읽기
                Rectangle rect = Screen.PrimaryScreen.Bounds;
                // 픽셀 포맷 정보 얻기 (Optional)
                int bitsPerPixel = Screen.PrimaryScreen.BitsPerPixel;
                PixelFormat pixelFormat = PixelFormat.Format32bppArgb;
                if (bitsPerPixel <= 16)
                {
                    pixelFormat = PixelFormat.Format16bppRgb565;
                }
                if (bitsPerPixel == 24)
                {
                    pixelFormat = PixelFormat.Format24bppRgb;
                }
                // 화면 크기만큼의 Bitmap 생성
                using (Bitmap bmp = new Bitmap(rect.Width, rect.Height, pixelFormat))
                {
                    // Bitmap 이미지 변경을 위해 Graphics 객체 생성
                    using (Graphics gr = Graphics.FromImage(bmp))
                    {
                        // 화면을 그대로 카피해서 Bitmap 메모리에 저장
                        gr.CopyFromScreen(rect.Left, rect.Top, 0, 0, rect.Size);
                    }
                    // 파일을 저장할 경로
                    string path = @"C:\Temp\client\screenshot.png";
                    // 해당 폴더가 없으면 생성
                    string directoryName = Path.GetDirectoryName(path);
                    if (!Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }
                    // Bitmap 데이타를 파일로 저장
                    bmp.Save(path);
                }
            }
            catch (Exception)
            {
                //MessageBox.Show($"Capture 예외 발생 : {ex.Message}");
            }
        }
    }
}
