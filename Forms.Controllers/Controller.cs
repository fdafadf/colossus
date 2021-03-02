using System;
using System.Configuration;
using System.Windows.Forms;

namespace C64.Chess.Forms.Controller
{
    abstract class Controller
    {
        public event Action StateChanged;
        public event Action Navigated;
        public event Action<Screen> Displayed;
        public bool IsWaitingForIteraction;
        public bool IsBusy;

        string state;
        protected Node currentNode = new Node();

        public string State
        {
            get => state;
            protected set
            {
                state = value;
                StateChanged?.Invoke();
            }
        }

        public Node CurrentNode
        {
            get => currentNode;
            protected set
            {
                currentNode = value;
                Navigated?.Invoke();
            }
        }

        public abstract void Start();

        public virtual void Play(Node node)
        {
        }

        public virtual void ClipboardPaste(string text)
        {
        }

        public virtual string GetNodeValue(Node node)
        {
            return node.Value.ToString();
        }

        protected void RaiseDisplayed(Screen screen)
        {
            Displayed?.Invoke(screen);
        }
    }
}
