using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
	public class MovementVectorsCalculator : BaseActionVectorsCalculator
	{
		public MovementVectorsCalculator(IBoard newBoard) : base(newBoard)
		{
		}

		protected override List<Vector2Int> GetProperActionVectorsFromPiece(Piece piece)
		{
			return piece.Behaviour.MovementVectors;
		}

		protected override ActionVectorAndItsTarget GetActionVectorAndItsTarget(Vector2Int actionVector, Piece piece, Vector2Int piecePosition)
		{
            List<Tile> tilesToLand = new List<Tile>();

            int i = 0;
            do
            {
                i++;
                var positionToConsider = piecePosition + actionVector * i;

                if (positionToConsider.x < 0 || positionToConsider.x >= board.SizeX ||
                    positionToConsider.y < 0 || positionToConsider.y >= board.SizeY)
                {
                    // This movement vector already exceeded the board.
                    break;
                }

                var tileToConsider = board.Tiles[positionToConsider.x, positionToConsider.y];

                if (tileToConsider.PieceOnTile != null)
                {
                    break;
                }

                tilesToLand.Add(tileToConsider);
            }
            while (piece.Behaviour.HasInfiniteMovementRange);

            return new ActionVectorAndItsTarget(tilesToLand, null);
        }
	}
}