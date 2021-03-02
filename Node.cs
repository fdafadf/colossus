using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;

namespace C64.Chess
{
    class Node
    {
        static int SearchingDepth = int.Parse(ConfigurationManager.AppSettings["SearchingDepth"]);

        //public readonly Node Parent;
        public readonly Move Move;
        public readonly State State;
        public List<Node> Children;
        public int Value;

        public Node()
        {
            State = new State();
        }

        public Node(Node parent, Move move, State state)
        {
            //Parent = parent;
            Move = move;
            State = state;
        }

        public Node FindChildren(string move)
        {
            var (fromX, fromY, toX, toY) = (move[0] - 'a', '8' - move[1], move[3] - 'a', '8' - move[4]);

            if (Children == null)
            {
                Expand();
            }

            return Children.FirstOrDefault(child => child.Move.Equals(fromX, fromY, toX, toY));
        }

        public void Expand()
        {
            if (Children == null)
            {
                Children = State.Children.Select(v => new Node(this, v.Key, v.Value)).ToList();
            }
        }

        private int Expand(int n)
        {
            if (n == 0)
            {
                return 1;
            }
            else
            {
                Expand();
                return Children.Select(child => child.Expand(n - 1)).Sum() + 1;
            }
        }

        #region MinMax

        private int MinMax()
        {
            int nodes = Expand(SearchingDepth);

            if (Children == null || Children.Count == 0)
            {
                Value = State.Value;
            }
            else
            {
                Value = int.MaxValue;
                Children.ForEach(child => { child.MinMax(); Value = Math.Min(Value, child.Value); });
                Value = -Value;
            }

            return nodes;
        }

        #endregion

        #region MinMax with AB

        private int MinMax(int n)
        {
            return MinMax(n, int.MinValue, int.MaxValue, State.Player.IsWhite);
        }

        private int MinMax(int n, int a, int b, bool max)
        {
            int visits = 1;

            if (n == 0)
            {
                Value = State.Value; 
            }
            else
            {
                if (Children == null)
                {
                    Expand();
                }

                if (max)
                {
                    Value = int.MinValue;

                    foreach (var child in Children)
                    {
                        visits += child.MinMax(n - 1, a, b, false);
                        Value = Math.Max(child.Value, Value);
                        a = Math.Max(a, Value);

                        if (b <= a)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    Value = int.MaxValue;

                    foreach (var child in Children)
                    {
                        visits += child.MinMax(n - 1, a, b, true);
                        Value = Math.Min(child.Value, Value);
                        a = Math.Min(a, Value);

                        if (b <= a)
                        {
                            break;
                        }
                    }
                }
            }

            return visits;
        }

        #endregion

        public int CalculateValue()
        {
            return MinMax(SearchingDepth);
        }
    }
}
