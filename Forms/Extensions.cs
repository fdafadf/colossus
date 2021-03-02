using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace C64.Chess.Forms
{
    static class Extensions
    {
        static public Font ChangeSize(this Font self, float size, GraphicsUnit unit)
        {
            if (self != null && self.Size != size)
            {
                return new Font(self.Name, size, self.Style, unit, self.GdiCharSet, self.GdiVerticalFont);
            }

            return self;
        }

        static public void DrawSymbol(this Graphics self, char c, Font font, RectangleF bounds, StringFormat stringFormat, Brush brush, Brush background)
        {
            var path = new GraphicsPath();
            path.AddString($"{c}", font.FontFamily, 0, font.Size * 0.99f, bounds, stringFormat);
            var iter = new GraphicsPathIterator(path);
            var subPath = new GraphicsPath();

            while (iter.NextSubpath(subPath, out var _) != 0)
            {
                self.FillRegion(background, new Region(subPath));
            }

            self.FillPath(Brush, path);
            self.DrawString($"{c}", font, brush, bounds, stringFormat);
        }

        static SolidBrush Brush = new SolidBrush(Color.FromArgb(128, 128, 128, 128));
    }
}
