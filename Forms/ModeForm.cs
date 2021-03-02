using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C64.Chess.Forms
{
    public partial class ModeForm : Form
    {
        public string Mode { get; private set; }

        public ModeForm()
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
        }

        private void buttonExplore_Click(object sender, EventArgs e)
        {
            Mode = "explore";
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            Mode = "play";
        }

        private void buttonColossius_Click(object sender, EventArgs e)
        {
            Mode = "colossus";
        }
    }
}
