using C64.Chess.Forms.Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace C64.Chess.Forms
{
    public partial class MainForm : Form, IMessageFilter
    {
        internal Controller.Controller Controller;

        public MainForm()
        {
            InitializeComponent();
            listViewMoves.Enabled = false;
            boardControl.PlayerPieceClick += OnPlayerPieceClick;
            boardControl.SelectedMovesClick += BoardControl1_SelectedMovesClick;
            Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
        }

        public bool PreFilterMessage(ref Message message)
        {
            if (message.Msg == 0x0100)
            {
                if ((int)message.WParam == 0x00000056 && ModifierKeys.HasFlag(Keys.Control))
                {
                    string clipboardText = Clipboard.GetText();

                    if (string.IsNullOrWhiteSpace(clipboardText) == false)
                    {
                        Controller.ClipboardPaste(clipboardText);
                    }
                }
            }

            return false;
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Controller.StateChanged += OnStateChanged;
            Controller.Navigated += OnNavigated;
            Controller.Displayed += OnControllerDisplayed;
            Controller.Start();
        }

        private void OnControllerDisplayed(Screen screen)
        {
            Invoke(() =>
            {
                if (screen != null)
                {
                    Screen targetScreen = Screen.AllScreens.FirstOrDefault(s => s.DeviceName != screen.DeviceName);

                    if (targetScreen != null)
                    {
                        Location = targetScreen.WorkingArea.Location;
                        WindowState = FormWindowState.Maximized;
                    }
                }
            });
        }

        private void OnStateChanged()
        {
            Invoke(() =>
            {
                labelStatus.Text = Controller.State;
                listViewMoves.Enabled = Controller.IsWaitingForIteraction;
                Cursor = Controller.IsBusy ? Cursors.WaitCursor : Cursors.Default;
            });
        }

        private void OnNavigated()
        {
            Invoke(() =>
            {
                RefreshBoard(Controller.CurrentNode);
                RefreshNavigationControls();
            });
        }

        void Invoke(Action action)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker) delegate { action(); });
            }
            else
            {
                action();
            }
        }

        private void RefreshBoard(Node node)
        {
            Invoke(() =>
            {
                boardControl.State = node.State;
                boardControl.Move = node.Move;
                boardControl.SelectedMoves = null;
                boardControl.Refresh();
            });
        }

        private void RefreshNavigationControls()
        {
            Invoke(() =>
            {
                ListViewItem CreateListViewItem(Node node)
                {
                    var figure = (node.State.Player.IsWhite ? BoardControl.UnicodeBlackSymbols : BoardControl.UnicodeWhiteSymbols)[node.Move.Figure];
                    var item = new ListViewItem(figure.ToString());
                    item.SubItems.Add(node.Move.ToString());
                    item.SubItems.Add(node.State.Value.ToString());
                    item.SubItems.Add(Controller.GetNodeValue(node));
                    item.Tag = node;
                    return item;
                }

                listViewMoves.Items.Clear();

                if (Controller.CurrentNode != null)
                {
                    listViewMoves.Items.AddRange(Controller.CurrentNode.Children.Select(CreateListViewItem).ToArray());
                }
            });
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            Controller.Play(listViewMoves.SelectedItems[0].Tag as Node);
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            boardControl.Move = Chess.Move.Empty;
            boardControl.SelectedMoves = listViewMoves.SelectedItems.OfType<ListViewItem>().Select(item => (item.Tag as Node).Move).ToArray();
            boardControl.Refresh();
        }

        private void OnPlayerPieceClick(int x, int y)
        {
            foreach (ListViewItem item in listViewMoves.Items)
            {
                item.Selected = (item.Tag as Node)?.Move.From(x, y) ?? false;
            }
        }

        private void BoardControl1_SelectedMovesClick(IEnumerable<Move> moves)
        {
            if (moves.Count() == 1)
            {
                var node = Controller.CurrentNode.FindChildren(moves.First().ToString());
                Controller.Play(node);
            }
        }
    }
}
