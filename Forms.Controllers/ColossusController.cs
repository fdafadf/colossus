using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using C64.Chess.Win32;

namespace C64.Chess.Forms.Controller
{
    class ColossusController : Controller
    {
        ColossusScreen ColossusScreen;

        public override async void Start()
        {
            await Task.Run(() =>
            {
                ColossusScreen = WaitForColossusScreen();
                RaiseDisplayed(Screen.FromHandle(ColossusScreen.Process.MainWindowHandle));
                ColossusScreen.WaitForMove();

                while (true)
                {
                    State = "Thinking...";
                    CurrentNode.CalculateValue();
                    CurrentNode = CurrentNode.Children.MinBy(child => child.Value);
                    State = "Playing...";
                    ColossusScreen.SendMove(CurrentNode.Move);
                    ColossusScreen.WaitForMove();
                    ColossusScreen.GetState();
                    Node nodeAfterColossusMove = null;

                    foreach (var child in CurrentNode.Children)
                    {
                        if (child.State.Player.Equals(ColossusScreen.PlayerState) && child.State.Opponent.Equals(ColossusScreen.OpponentState))
                        {
                            nodeAfterColossusMove = child;
                            break;
                        }
                    }

                    CurrentNode = nodeAfterColossusMove;
                }
            });
        }

        ColossusScreen WaitForColossusScreen()
        {
            State = "Loading Colossus Chess...";
            Process process = Process.GetProcessesByName("CCS64").FirstOrDefault();

            if (process != null)
            {
                var dialogResult = MessageBox.Show("Process CSS64 found. Do you want to use it instead of start a new?", "Loading Colossus Chess", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.No)
                {
                    process = null;
                }
            }

            if (process == null)
            {
                string emulatorPath = Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["CCS64ExecutablePath"]);
                string romPath = Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["COLOSS20Path"]);
                process = Process.Start(emulatorPath, $@"""{romPath}""");
            }

            return new ColossusScreen(process);
        }
    }
}
