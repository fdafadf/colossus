using System;

namespace C64.Chess.Win32
{
    //[StructLayout(LayoutKind.Sequential)]
    //public struct INPUT
    //{
    //    internal uint type;
    //    internal InputUnion U;
    //    internal static int Size
    //    {
    //        get { return Marshal.SizeOf(typeof(INPUT)); }
    //    }
    //}

    struct KeyboardInput
    {
        public UInt16 Vk;
        public UInt16 Scan;
        public UInt32 Flags;
        public UInt32 Time;
        public IntPtr ExtraInfo;
    }
}
