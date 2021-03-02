using System.Collections.Generic;

namespace C64.Chess
{
    public class PotentialMoves : List<Move>
    {
        public readonly PlayerState Fields;
        public readonly PlayerState OpponentFields;
        public readonly bool[,] InRange;

        public PotentialMoves(PlayerState fields, PlayerState opponentFields)
        {
            Fields = fields;
            OpponentFields = opponentFields;
            InRange = new bool[8, 8];

            for (byte y = 0; y < 8; y++)
            {
                for (byte x = 0; x < 8; x++)
                {
                    switch (Fields[x, y])
                    {
                        case FieldState.Knight:
                            AddSkoczekMoves(x, y);
                            break;
                        case FieldState.Rook:
                            AddWiezaMoves(x, y);
                            break;
                        case FieldState.Bishop:
                            AddGoniecMoves(x, y);
                            break;
                        case FieldState.Queen:
                            AddWiezaMoves(x, y);
                            AddGoniecMoves(x, y);
                            break;
                        case FieldState.Pawn:
                            AddPionMoves(x, y);
                            break;
                        case FieldState.King:
                            AddHetmanMoves(x, y);
                            break;
                    }
                }
            }
        }

        void AddPionMoves(int x, int y)
        {
            int d = Fields.IsWhite ? -1 : 1;
            
            if (AddPionMove(x, y, x, y + d) && y == (Fields.IsWhite ? 6 : 1))
            {
                AddPionMove(x, y, x, y + d + d);
            }

            AddPionMove2(x, y, x - 1, y + d);
            AddPionMove2(x, y, x + 1, y + d);
        }

        bool AddPionMove(int x, int y, int tx, int ty)
        {
            bool result = Inside(tx, ty) && Fields[tx, ty] == FieldState.Empty && OpponentFields[tx, ty] == FieldState.Empty;
            
            if (result)
            {
                AddMove(x, y, tx, ty);
            }

            return result;
        }

        void AddPionMove2(int x, int y, int tx, int ty)
        {
            if (Inside(tx, ty) && Fields[tx, ty] == FieldState.Empty && OpponentFields[tx, ty] != FieldState.Empty)
            {
                AddMove(x, y, tx, ty);
            }
        }

        void AddHetmanMoves(int x, int y)
        {
            AddMoves(x, y, (x - 1, y - 1), (x - 1, y + 0), (x - 1, y + 1), (x + 0, y - 1), (x + 0, y + 1), (x + 1, y - 1), (x + 1, y + 0), (x + 1, y + 1));
        }

        void AddGoniecMoves(int x, int y)
        {
            for (int tx = x + 1, ty = y + 1; tx < 8 && ty < 8 && AddLinearMove(x, y, tx, ty); tx++, ty++) ;
            for (int tx = x + 1, ty = y - 1; tx < 8 && ty >= 0 && AddLinearMove(x, y, tx, ty); tx++, ty--) ;
            for (int tx = x - 1, ty = y - 1; tx >= 0 && ty >= 0 && AddLinearMove(x, y, tx, ty); tx--, ty--) ;
            for (int tx = x - 1, ty = y + 1; tx >= 0 && ty < 8 && AddLinearMove(x, y, tx, ty); tx--, ty++) ;
        }

        void AddWiezaMoves(int x, int y)
        {
            for (int tx = x + 1; tx < 8 && AddLinearMove(x, y, tx, y); tx++) ;
            for (int tx = x - 1; tx >= 0 && AddLinearMove(x, y, tx, y); tx--) ;
            for (int ty = y + 1; ty < 8 && AddLinearMove(x, y, x, ty); ty++) ;
            for (int ty = y - 1; ty >= 0 && AddLinearMove(x, y, x, ty); ty--) ;
        }

        void AddSkoczekMoves(int x, int y)
        {
            AddMoves(x, y, (x + 2, y + 1), (x + 2, y - 1), (x + 1, y + 2), (x + 1, y - 2), (x - 1, y + 2), (x - 1, y - 2), (x - 2, y + 1), (x - 2, y - 1));
        }

        bool AddLinearMove(int x, int y, int tx, int ty)
        {
            if (Fields[tx, ty] != 0)
            {
                return false;
            }

            Add(x, y, tx, ty);
            return OpponentFields[tx, ty] == FieldState.Empty;
        }

        void AddMoves(int x, int y, params (int x, int y)[] t)
        {
            for (int i = 0; i < t.Length; i++)
            {
                AddMove(x, y, t[i].x, t[i].y);
            }
        }

        void AddMove(int x, int y, int tx, int ty)
        {
            if (Inside(tx, ty) && Fields[tx, ty] == FieldState.Empty)
            {
                Add(x, y, tx, ty);
            }
        }

        void Add(int x, int y, int tx, int ty)
        {
            InRange[tx, ty] = true;
            byte figure = Fields[x, y];

            if (figure == FieldState.Pawn)
            {
                if (Fields.IsWhite ? ty == 0 : ty == 7)
                {
                    Add(new Move(x, y, tx, ty, FieldState.Bishop));
                    Add(new Move(x, y, tx, ty, FieldState.Queen));
                    Add(new Move(x, y, tx, ty, FieldState.Knight));
                    Add(new Move(x, y, tx, ty, FieldState.Rook));
                }
                else
                {
                    Add(new Move(x, y, tx, ty, FieldState.Pawn));
                }
            }
            else
            {
                Add(new Move(x, y, tx, ty, figure));
            }
        }

        bool Inside(int x, int y) => x >= 0 && y >= 0 && x < 8 && y < 8;
    }
}
