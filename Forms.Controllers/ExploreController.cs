using System;
using System.Linq;
using System.Windows.Forms;

namespace C64.Chess.Forms.Controller
{
    class ExploreController : InteractiveController
    {
        public override void ClipboardPaste(string text)
        {
            try
            {
                Play(text.Split(' ').Aggregate(new Node(), (node, move) => node.FindChildren(move)));
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
    }
}
