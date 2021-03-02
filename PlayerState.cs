using System;

namespace C64.Chess
{
    public class PlayerState
    {
        byte[] FiguresStartConfiguration = { FieldState.Rook, FieldState.Knight, FieldState.Bishop, FieldState.Queen, FieldState.King, FieldState.Bishop, FieldState.Knight, FieldState.Rook };
        int[] Values = { 0, 1, 3, 3, 5, 9, 0 };

        readonly byte[,] fields;
        public readonly PieceColor Color;
        public readonly int KingX;
        public readonly int KingY;
        public readonly int Value;
        public byte this[int x, int y] => fields[x, y];
        public bool IsWhite => Color == PieceColor.White;

        public PlayerState(PieceColor color)
        {
            fields = new byte[8, 8];
            Value = Values[FieldState.Rook] * 2 + Values[FieldState.Bishop] * 2 + Values[FieldState.Knight] * 2 + Values[FieldState.Queen] + Values[FieldState.King] + Values[FieldState.Pawn] * 8;
            Color = color;
            KingX = 4;
            KingY = IsWhite ? 7 : 0;

            for (int i = 0; i < 8; i++)
            {
                fields[i, KingY] = FiguresStartConfiguration[i];
                fields[i, IsWhite ? 6 : 1] = FieldState.Pawn;
            }
        }

        public bool Equals(byte[,] state)
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    if (fields[x, y] != state[x, y])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private PlayerState(PlayerState state, int clearX, int clearY)
        {
            Color = state.Color;

            if (state.fields[clearX, clearY] == FieldState.Empty)
            {
                fields = state.fields;
                Value = state.Value;
            }
            else
            {
                fields = (byte[,])state.fields.Clone();
                Value = state.Value - Values[fields[clearX, clearY]];
                fields[clearX, clearY] = FieldState.Empty;
            }

            (KingX, KingY) = (state.KingX, state.KingY);
        }

        private PlayerState(PlayerState state, Move move)
        {
            Color = state.Color;
            fields = (byte[,])state.fields.Clone();
            Value = state.Value + Values[move.Figure] - Values[fields[move.FromX, move.FromY]];
            fields[move.ToX, move.ToY] = move.Figure;
            fields[move.FromX, move.FromY] = FieldState.Empty;
            bool isKingMove = state.KingX == move.FromX && state.KingY == move.FromY;
            (KingX, KingY) = isKingMove ? (move.ToX, move.ToY) : (state.KingX, state.KingY);
        }

        public PlayerState Play(Move move) => new PlayerState(this, move);

        public PlayerState Clear(Move move) => new PlayerState(this, move.ToX, move.ToY);
    }
}
