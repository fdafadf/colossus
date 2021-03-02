using C64.Chess.Forms;
using C64.Chess.Forms.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C64.Chess
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string mode;
            Controller controller = null;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length == 1)
            {
                mode = args[0];
            }
            else
            {
                var modeForm = new ModeForm();
                
                if (modeForm.ShowDialog() == DialogResult.OK)
                {
                    mode = modeForm.Mode;
                }
                else
                {
                    return;
                }
            }

            switch (mode)
            {
                case "play":
                    controller = new BotController();
                    break;
                case "colossus":
                    controller = new ColossusController();
                    break;
                case "explore":
                    controller = new ExploreController();
                    break;
                default:
                    MessageBox.Show($@"Unknown mode ""{args[0]}"".");
                    break;
            }

            if (controller != null)
            {
                var form = new MainForm();
                form.Controller = controller;
                Application.AddMessageFilter(form);
                Application.Run(form);
            }
        }
    }
}
