using System;
using System.Threading;
using System.Threading.Tasks;

namespace C64.Chess.Forms.Controller
{
    abstract class InteractiveController : Controller
    {
        ManualResetEvent playEvent = new ManualResetEvent(false);

        public override async void Start()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    IsWaitingForIteraction = false;
                    IsBusy = true;
                    State = "Searching...";
                    DateTime startTime = DateTime.Now;
                    int nodes = CurrentNode.CalculateValue();
                    OnExpanded();
                    IsWaitingForIteraction = true;
                    IsBusy = false;
                    State = $"{nodes} tree nodes were searched in time {DateTime.Now - startTime}.";
                    playEvent.WaitOne();
                    playEvent.Reset();
                }
            });
        }

        public override void Play(Node node)
        {
            node.Expand();
            CurrentNode = node;
            playEvent.Set();
        }

        protected virtual void OnExpanded()
        { 
            CurrentNode = CurrentNode;
        }
    }
}
