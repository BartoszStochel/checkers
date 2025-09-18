using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
	public class AttackVectorsCalculator : BaseActionVectorsCalculator
	{
		public AttackVectorsCalculator(IBoard newBoard) : base(newBoard)
		{
		}

		protected override List<Vector2Int> GetProperActionVectorsFromPiece(Piece piece)
		{
			return piece.Behaviour.AttackVectors;
		}

        protected override ActionVectorAndItsTarget GetActionVectorAndItsTarget(Vector2Int actionVector, Piece piece, Vector2Int piecePosition)
        {
            Piece enemyPieceToAttack = null;
            List<Tile> tilesToLandAfterAttack = new List<Tile>();

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
                    if (enemyPieceToAttack != null)
                    {
                        // There is another piece after attack target, the move is blocked now.
                        break;
                    }
                    else if (tileToConsider.PieceOnTile.IsWhite != piece.IsWhite)
                    {
                        // We have encountered first enemy piece, this is our target.
                        enemyPieceToAttack = tileToConsider.PieceOnTile;
                    }
                    else
                    {
                        // First piece we have envountered is our ally, attack is canceled.
                        break;
                    }
                }
                else
                {
                    if (enemyPieceToAttack != null)
                    {
                        tilesToLandAfterAttack.Add(tileToConsider);
                    }
                }
            }
            while ((i == 1 && enemyPieceToAttack != null) || piece.Behaviour.HasInfiniteMovementRange);

            return new ActionVectorAndItsTarget(tilesToLandAfterAttack, enemyPieceToAttack);
        }
    }
}