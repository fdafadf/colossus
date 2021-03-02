using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

namespace C64.Chess.Forms
{
    class BoardControl : Control
    {
        public const string UnicodeWhiteSymbols = " ♙♗♘♖♕♔";
        public const string UnicodeBlackSymbols = " ♟♝♞♜♛♚";
        public State State = new State();
        public Move Move;
        public Move[] SelectedMoves;
        public Pen MovePen = new Pen(Color.FromArgb(255, 0, 0, 255), 2);
        public Brush BlackFieldBrush = new SolidBrush(Color.FromArgb(153, 187, 173));
        public Brush WhiteFieldBrush = new SolidBrush(Color.FromArgb(235, 216, 183));
        public Brush SelectedBlackFieldBrush = new SolidBrush(Color.FromArgb(154, 129, 148));
        public Brush SelectedWhiteFieldBrush = new SolidBrush(Color.FromArgb(198, 169, 163));
        public event Action<int, int> PlayerPieceClick;
        public event Action<IEnumerable<Move>> SelectedMovesClick;

        public BoardControl()
        {
            DoubleBuffered = true;
        }

        StringFormat StringFormat = new StringFormat()
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Near
        };

        protected override void OnClick(EventArgs e)
        {
            if (e is MouseEventArgs mouseEvent)
            {
                float width = (Width - 1) / 8.0f;
                float height = (Height - 1) / 8.0f;
                int x = (int)(mouseEvent.X / width);
                int y = (int)(mouseEvent.Y / height);

                if (State.Player[x, y] == FieldState.Empty)
                {
                    var moves = SelectedMoves?.Where(m => m.To(x, y));

                    if (moves?.Any() ?? false)
                    {
                        SelectedMovesClick?.Invoke(moves);
                    }
                }
                else
                {
                    PlayerPieceClick?.Invoke(x, y);
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            float width = (Width - 1) / 8.0f;
            float height = (Height - 1) / 8.0f;
            Font = Font.ChangeSize(height, GraphicsUnit.Pixel); 
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            RectangleF bounds = new RectangleF(0, 0, width, height);
            (string playerSymbols, string opponentSymbols) = State.Player.IsWhite ? (UnicodeWhiteSymbols, UnicodeBlackSymbols) : (UnicodeBlackSymbols, UnicodeWhiteSymbols);

            for (int y = 0; y < 8; y++)
            {
                bounds.X = 0;

                for (int x = 0; x < 8; x++)
                {
                    bool isSelected = Move.IsEmpty == false && (Move.From(x, y) || Move.To(x, y))
                        || (SelectedMoves?.Any(m => m.From(x, y) || m.To(x, y)) ?? false);

                    if (isSelected)
                    {
                        e.Graphics.FillRectangle(((x + y) % 2 == 0) ? SelectedWhiteFieldBrush : SelectedBlackFieldBrush, bounds);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(((x + y) % 2 == 0) ? WhiteFieldBrush : BlackFieldBrush, bounds);
                    }

                    bounds.X += width;
                }

                bounds.Y += height;
            }

            bounds.Y = 0;

            for (int y = 0; y < 8; y++)
            {
                bounds.X = 0;

                for (int x = 0; x < 8; x++)
                {
                    if (State.Player[x, y] != 0)
                    {
                        e.Graphics.DrawSymbol(playerSymbols[State.Player[x, y]], Font, bounds, StringFormat, Brushes.Black, Brushes.White);
                    }
                    else if (State.Opponent[x, y] != 0)
                    {
                        e.Graphics.DrawSymbol(opponentSymbols[State.Opponent[x, y]], Font, bounds, StringFormat, Brushes.Black, Brushes.White);
                    }

                    bounds.X += width;
                }

                bounds.Y += height;
            }

            e.Graphics.DrawRectangle(Pens.Black, 0, 0, Width - 1, Height - 1);
        }
    }
}
