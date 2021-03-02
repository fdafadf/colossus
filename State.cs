using System.Collections.Generic;
using System.Linq;

namespace C64.Chess
{
    public class State
    {
        public readonly PlayerState Player;
        public readonly PlayerState Opponent;
        public readonly PotentialMoves PotentialMoves;
        public readonly bool Allowed;
        public readonly int Value;
        Dictionary<Move, State> children;

        public State()
        {
            Player = new PlayerState(PieceColor.White);
            Opponent = new PlayerState(PieceColor.Black);
            PotentialMoves = new PotentialMoves(Player, Opponent);
            Allowed = PotentialMoves.InRange[Opponent.KingX, Opponent.KingY] == false;
            Value = Player.Value - Opponent.Value;
        }

        public State(State state, Move move)
        {
            Player = state.Opponent.Clear(move);
            Opponent = state.Player.Play(move);
            PotentialMoves = new PotentialMoves(Player, Opponent);
            Allowed = PotentialMoves.InRange[Opponent.KingX, Opponent.KingY] == false;
            Value = Player.IsWhite ? Player.Value - Opponent.Value : Opponent.Value - Player.Value;
        }

        public Dictionary<Move, State> Children => children = children ?? PotentialMoves
                        .Select(move => (move, state: new State(this, move)))
                        .Where(_ => _.state.Allowed)
                        .ToDictionary(_ => _.move, _ => _.state);

        public bool IsFinal => Children.Count == 0;
    }
}
