using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C64.Chess.Win32
{
    static class Extensions
    {
        public static readonly HashSet<Keys> ExtendedKeys = new HashSet<Keys>
        (
            new Keys[]
            {
                Keys.Up, Keys.Down, Keys.Left, Keys.Right
            }
        );

        public static void SendKeys(this Process self, params Keys[] keys)
        {
            User32.SetForegroundWindow(self.MainWindowHandle);
            Thread.Sleep(200);

            foreach (var key in keys)
            {
                uint flags = 0x0008;

                if (ExtendedKeys.Contains(key))
                {
                    flags |= 0x0001;
                }

                User32.SendKeyboardInput((uint)key, flags);
                Thread.Sleep(100);
                User32.SendKeyboardInput((uint)key, flags | 0x0002); 
                Thread.Sleep(500);
            }
        }

        public static Image CaptureWindowImage(this Process self)
        {
            return CaptureWindowImageFromHandle(self.MainWindowHandle);
        }

        public static Image CaptureWindowImageFromHandle(IntPtr handle)
        {
            IntPtr hdcSrc = User32.GetWindowDC(handle);
            Rect windowRect = new Rect();
            User32.GetWindowRect(handle, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            IntPtr hdcDest = Gdi32.CreateCompatibleDC(hdcSrc);
            IntPtr hBitmap = Gdi32.CreateCompatibleBitmap(hdcSrc, width, height);
            IntPtr hOld = Gdi32.SelectObject(hdcDest, hBitmap);
            Gdi32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, Gdi32.SRCCOPY);
            Gdi32.SelectObject(hdcDest, hOld);
            Gdi32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);
            Image img = Image.FromHbitmap(hBitmap);
            Gdi32.DeleteObject(hBitmap);
            return img;
        }
    }
}
