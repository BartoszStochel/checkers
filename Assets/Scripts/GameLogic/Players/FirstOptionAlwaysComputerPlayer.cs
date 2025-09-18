using System.Collections.Generic;

namespace GameLogic
{
    public class FirstOptionAlwaysComputerPlayer : BaseComputerPlayer
    {
        public override (Piece pieceToPerformAction, Tile landingTile, Piece attackTarget) GetActionData(List<PieceAndItsActionVectors> possibleActors)
        {
            var randomPieceToPerformAction = possibleActors[0];
            var randomAttackVector = randomPieceToPerformAction.ActionVectors[0];
            var randomTileToLand = randomAttackVector.LandingTargets[0];

            return (randomPieceToPerformAction.Piece, randomTileToLand, randomAttackVector.AttackTarget);
        }
    }
}