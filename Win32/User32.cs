using System;
using System.Runtime.InteropServices;

namespace C64.Chess.Win32
{
    class User32
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);
        [DllImport("User32.dll")]
        public static extern int SetForegroundWindow(IntPtr point);
        [DllImport("user32.dll")]
        static extern uint MapVirtualKey(uint uCode, uint uMapType);
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
        [DllImport("user32.dll")]
        private static extern IntPtr GetMessageExtraInfo();
        //[DllImport("user32.dll")]
        //internal static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern UInt32 SendInput(UInt32 numberOfInputs, Input[] inputs, Int32 sizeOfInputStructure);

        public const byte VK_RETURN = 0x0D;
        public const byte VK_LEFT = 0x25;
        public const byte VK_UP = 0x26;
        public const byte VK_RIGHT = 0x27;
        public const byte VK_DOWN = 0x28;

        public const int KEYEVENTF_EXTENDEDKEY = 0x1;
        public const int KEYEVENTF_KEYUP = 0x2;

        public static void SendKeyboardInput(uint uCode, uint flags)
        {
            byte bScan = (byte)MapVirtualKey(uCode, 0);
            var input = new Input();
            input.Type = (uint)InputType.Keyboard;
            input.Data.Keyboard = new KeyboardInput()
            {
                Vk = 0,
                Scan = bScan,
                Flags = flags,
                Time = 0,
                ExtraInfo = GetMessageExtraInfo()
            };
            Input[] inputList = new Input[] { input };
            SendInput((uint)inputList.Length, inputList, Marshal.SizeOf(typeof(Input)));
        }
    }
}
