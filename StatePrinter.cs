using System;

namespace C64.Chess
{
    class StatePrinter
    {
        public void Print(State state)
        {
            ConsoleColor originalForegroundColor = Console.ForegroundColor;
            ConsoleColor originalBackgroundColor = Console.BackgroundColor;
            PlayerState whiteFields = state.Player.IsWhite ? state.Player : state.Opponent;
            PlayerState blackFields = state.Player.IsWhite ? state.Opponent : state.Player;
            Console.WriteLine("  a b c d e f g h");

            for (int y = 0; y < 8; y++)
            {
                Console.Write(8 - y);

                try
                {
                    for (int x = 0; x < 8; x++)
                    {
                        byte fieldState = FieldState.Empty;
                        Console.BackgroundColor = ConsoleColor.DarkCyan;
                        Console.Write(' ');

                        if (whiteFields[x, y] != FieldState.Empty)
                        {
                            fieldState = whiteFields[x, y];
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.BackgroundColor = ConsoleColor.White;
                        }
                        else if (blackFields[x, y] != FieldState.Empty)
                        {
                            fieldState = blackFields[x, y];
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.BackgroundColor = ConsoleColor.Black;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.BackgroundColor = ConsoleColor.DarkCyan;
                        }

                        Console.Write(FieldState.AsciiSymbols[fieldState]);
                    }

                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    Console.Write(' ');
                }
                finally
                {
                    Console.ForegroundColor = originalForegroundColor;
                    Console.BackgroundColor = originalBackgroundColor;
                    Console.WriteLine('|');
                }
            }
        }
    }
}
