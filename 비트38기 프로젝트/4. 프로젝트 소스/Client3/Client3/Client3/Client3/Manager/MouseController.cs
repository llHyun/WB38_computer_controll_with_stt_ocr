//MouserController.cs
using System;
using System.Runtime.InteropServices;
using System.Threading; 

namespace Client3
{
    public class MouseController
    {   
        //마우스 이벤트 받아오기
        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

        public void Move(int x, int y)
        {
            try
            {
                SetCursorPos(x, y);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Move 메서드에서 예외가 발생 : {ex}");
            }
        }

        public void Click()
        {
            try
            {
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Click 메서드에서 예외가 발생 : {ex}");
            }
        }

        public void DoubleClick()
        {
            try
            {
                Click();
                Thread.Sleep(200); // 일반적으로 더블 클릭 간격은 200ms
                Click();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DoubleClick 메서드에서 예외가 발생 : {ex}");
            }
        }
    }
}
