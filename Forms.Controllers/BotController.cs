using System.Windows.Forms;

namespace C64.Chess.Forms.Controller
{
    class BotController : InteractiveController
    {
        protected override void OnExpanded()
        {
            var node = CurrentNode.Children.MinBy(child => -child.Value);

            if (node == null)
            {
                MessageBox.Show("Resign");
            }
            else
            {
                CurrentNode = node;
            }
        }

        public override string GetNodeValue(Node node) => "";
    }
}
