using C64.Chess.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace C64.Chess.Forms.Controller
{
    class ColossusScreen
    {
        static Bitmap FiguresImage = Properties.Resources.Colossus;
        static Dictionary<uint, byte> PlayerHashes = new Dictionary<uint, byte>();
        static Dictionary<uint, byte> OpponentHashes = new Dictionary<uint, byte>();

        static ColossusScreen()
        {
            OpponentHashes.Add(16711743, FieldState.Rook);
            OpponentHashes.Add(329728, FieldState.Knight);
            OpponentHashes.Add(16521599, FieldState.Bishop);
            OpponentHashes.Add(1048616, FieldState.Queen);
            OpponentHashes.Add(14689343, FieldState.King);
            OpponentHashes.Add(268608, FieldState.Bishop);
            OpponentHashes.Add(16582783, FieldState.Knight);
            OpponentHashes.Add(65568, FieldState.Rook);
            OpponentHashes.Add(32768, FieldState.Pawn);
            OpponentHashes.Add(16744959, FieldState.Pawn);
            OpponentHashes.Add(16777215, FieldState.Empty);
            OpponentHashes.Add(0, FieldState.Empty);
            PlayerHashes.Add(65472, FieldState.Rook);
            PlayerHashes.Add(16447487, FieldState.Knight);
            PlayerHashes.Add(255616, FieldState.Bishop);
            PlayerHashes.Add(15728599, FieldState.Queen);
            PlayerHashes.Add(2087872, FieldState.King);
            PlayerHashes.Add(16508607, FieldState.Bishop);
            PlayerHashes.Add(194432, FieldState.Knight);
            PlayerHashes.Add(16711647, FieldState.Rook);
            PlayerHashes.Add(16744447, FieldState.Pawn);
            PlayerHashes.Add(32256, FieldState.Pawn);
            PlayerHashes.Add(16777215, FieldState.Empty);
            PlayerHashes.Add(0, FieldState.Empty);
        }

        public readonly Process Process;
        public byte[,] PlayerState;
        public byte[,] OpponentState;
        Bitmap LastScreenshot;

        public ColossusScreen(Process process)
        {
            Process = process;
        }

        public void SendMove(Move move)
        {
            int x = 0;
            int y = 7;

            int Move(int v, int tv, Keys increaseKey, Keys decreaseKey)
            {
                while (v != tv)
                {
                    if (v < tv)
                    {
                        Process.SendKeys(increaseKey);
                        v++;
                    }
                    else
                    {
                        Process.SendKeys(decreaseKey);
                        v--;
                    }
                }

                return v;
            }

            x = Move(x, move.FromX, Keys.Right, Keys.Left);
            y = Move(y, move.FromY, Keys.Down, Keys.Up);
            Process.SendKeys(Keys.Return);
            Move(x, move.ToX, Keys.Right, Keys.Left);
            Move(y, move.ToY, Keys.Down, Keys.Up);
            Process.SendKeys(Keys.Return);
        }

        public void WaitForMove()
        {
            do
            {
                Thread.Sleep(200);
                TakeScreenshot();
            }
            while (LastScreenshotContains(50, 278, 2, 178, 90, 4) == false);
        }

        public void GetState()
        {
            TakeScreenshot();
            PlayerState = new byte[8, 8];
            OpponentState = new byte[8, 8];

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    uint checksum = 0;
                    uint bitValue = 1;
                    int fieldX = 50 + 24 * x;
                    int fieldY = 102 + 20 * y;

                    for (int i = 0; i < 24; i++)
                    {
                        if (LastScreenshot.GetPixel(fieldX + i, fieldY + (i > 9 ? 9 : i)).B > 128)
                        {
                            checksum += bitValue;
                        }

                        bitValue *= 2;
                    }

                    if (PlayerHashes.TryGetValue(checksum, out byte fieldState))
                    {
                        PlayerState[x, y] = fieldState;
                    }
                    else if (OpponentHashes.TryGetValue(checksum, out fieldState))
                    {
                        OpponentState[x, y] = fieldState;
                    }
                    else
                    {
                        MessageBox.Show($"Unknown state {checksum} at {x} {y}");
                    }
                }
            }
        }

        void TakeScreenshot()
        {
            LastScreenshot = Process.CaptureWindowImage() as Bitmap;
        }

        bool LastScreenshotContains(int screenX, int screenY, int figureX, int figureY, int width, int height)
        {
            if (LastScreenshot.Width < 400 || LastScreenshot.Height < 360)
            {
                return false;
            }

            for (int py = 0; py < height; py++)
            {
                for (int px = 0; px < width; px++)
                {
                    var p1 = LastScreenshot.GetPixel(screenX + px, screenY + py);
                    var p2 = FiguresImage.GetPixel(figureX + px, figureY + py);

                    if (p1.ToArgb() != p2.ToArgb())
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
