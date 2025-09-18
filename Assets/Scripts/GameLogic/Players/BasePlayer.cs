using System.Collections.Generic;

namespace GameLogic
{
    public abstract class BasePlayer : IPlayer
    {
        public abstract bool IsHumanPlayer { get; }
        public List<Piece> Pieces { get; private set; }

        public void SetNewPieces(List<Piece> newPieces)
        {
            Pieces = newPieces;
        }

        public abstract (Piece pieceToPerformAction, Tile landingTile, Piece attackTarget) GetActionData(List<PieceAndItsActionVectors> possibleActors);
    }
}