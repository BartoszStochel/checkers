using System.Collections.Generic;

namespace GameLogic
{
    public interface IActionVectorsCalculator
    {
        List<PieceAndItsActionVectors> GetPlayerPiecesThatCanActNow(IPlayer player);
        List<PieceAndItsActionVectors> GetPiecesThatCanActNow(List<Piece> possibleActors);
        List<ActionVectorAndItsTarget> GetActionVectorsOfPiece(Piece piece);
    }
}