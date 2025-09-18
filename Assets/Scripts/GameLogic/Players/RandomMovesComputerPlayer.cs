using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class RandomMovesComputerPlayer : BaseComputerPlayer
    {
        public override (Piece pieceToPerformAction, Tile landingTile, Piece attackTarget) GetActionData(List<PieceAndItsActionVectors> possibleActors)
        {
            var randomPieceToPerformAction = possibleActors[Random.Range(0, possibleActors.Count)];
            var randomAttackVector = randomPieceToPerformAction.ActionVectors[Random.Range(0, randomPieceToPerformAction.ActionVectors.Count)];
            var randomTileToLand = randomAttackVector.LandingTargets[Random.Range(0, randomAttackVector.LandingTargets.Count)];

            return (randomPieceToPerformAction.Piece, randomTileToLand, randomAttackVector.AttackTarget);
        }
    }
}