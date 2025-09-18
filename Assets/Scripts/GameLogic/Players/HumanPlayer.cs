using System.Collections.Generic;

namespace GameLogic
{
    public class HumanPlayer : BasePlayer
    {
        public override bool IsHumanPlayer => true;

        public override (Piece pieceToPerformAction, Tile landingTile, Piece attackTarget) GetActionData(List<PieceAndItsActionVectors> possibleActors)
		{
            return (null, null, null);
        }
    }
}