using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public abstract class BaseActionVectorsCalculator : IActionVectorsCalculator
    {
        protected IBoard board;

        protected abstract List<Vector2Int> GetProperActionVectorsFromPiece(Piece piece);
        protected abstract ActionVectorAndItsTarget GetActionVectorAndItsTarget(Vector2Int actionVector, Piece piece, Vector2Int piecePosition);

        public BaseActionVectorsCalculator(IBoard newBoard)
		{
            board = newBoard;
		}

        public List<PieceAndItsActionVectors> GetPlayerPiecesThatCanActNow(IPlayer player)
        {
            return GetPiecesThatCanActNow(player.Pieces);
        }

        public List<PieceAndItsActionVectors> GetPiecesThatCanActNow(List<Piece> possibleActors)
		{
            var piecesAndTargets = new List<PieceAndItsActionVectors>();

            for (int i = 0; i < possibleActors.Count; i++)
            {
                var actionVectors = GetActionVectorsOfPiece(possibleActors[i]);

                if (actionVectors.Count > 0)
                {
                    piecesAndTargets.Add(new PieceAndItsActionVectors(possibleActors[i], actionVectors));
                }
            }

            return piecesAndTargets;
        }

        public List<ActionVectorAndItsTarget> GetActionVectorsOfPiece(Piece piece)
        {
            var piecePosition = board.GetPiecePositionOnBoard(piece);
            var actionVectors = new List<ActionVectorAndItsTarget>();

            for (int i = 0; i < GetProperActionVectorsFromPiece(piece).Count; i++)
            {
                var actionVectorAndItsTarget = GetActionVectorAndItsTarget(GetProperActionVectorsFromPiece(piece)[i], piece, piecePosition);

                if (actionVectorAndItsTarget.LandingTargets.Count > 0)
                {
                    actionVectors.Add(actionVectorAndItsTarget);
                }
            }

            return actionVectors;
        }
	}
}