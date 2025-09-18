using System.Collections.Generic;

namespace GameLogic
{
    public interface IPlayer
    {
        bool IsHumanPlayer { get; }
        List<Piece> Pieces { get; }

        void SetNewPieces(List<Piece> newPieces);
        (Piece pieceToPerformAction, Tile landingTile, Piece attackTarget) GetActionData(List<PieceAndItsActionVectors> possibleActors);
    }
}