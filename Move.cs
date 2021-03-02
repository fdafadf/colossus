using System.Runtime.InteropServices;

namespace C64.Chess
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Move 
    {
        const string XSymbols = "abcdefgh";
        const string YSymbols = "87654321";

        public static implicit operator Move(int d) => d;
        public static readonly Move Empty = new Move(0, 0, 0, 0, 0);

        public byte FromX => (byte)(Value & 0x0f);
        public byte FromY => (byte)((Value & 0xf0) >> 4);
        public byte ToX => (byte)((Value & 0xf00) >> 8);
        public byte ToY => (byte)((Value & 0xf000) >> 12);
        public byte Figure => (byte)((Value & 0xf0000) >> 16);
        [FieldOffset(0)]
        public int Value;

        public Move(int fromX, int fromY, int toX, int toY, byte figure)
        {
            Value = fromX + (fromY << 4) + (toX << 8) + (toY << 12) + (figure << 16);
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public override string ToString()
        {
            return $"{XSymbols[FromX]}{YSymbols[FromY]}-{XSymbols[ToX]}{YSymbols[ToY]}";
        }

        public bool IsEmpty => FromX == ToX && FromY == ToY;
        public bool From(int x, int y) => FromX == x && FromY == y;
        public bool To(int x, int y) => ToX == x && ToY == y;
        public bool Equals(int fromX, int fromY, int toX, int toY) => FromX == fromX && FromY == fromY && ToX == toX && ToY == toY;
    }
}
