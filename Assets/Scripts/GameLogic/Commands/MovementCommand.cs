using UnityEngine;
using System;

namespace GameLogic
{
	public class MovementCommand : ICommand
	{
		private Piece movedPiece;
		private Tile startingTile;
		private Tile finishTile;
		private Action<Piece, Tile> eventPieceMovedToAnotherTile;

		public MovementCommand(Piece movedPiece, Tile startingTile, Tile finishTile, Action<Piece, Tile> eventPieceMovedToAnotherTile)
		{
			this.movedPiece = movedPiece;
			this.startingTile = startingTile;
			this.finishTile = finishTile;
			this.eventPieceMovedToAnotherTile = eventPieceMovedToAnotherTile;
		}

		public void Execute()
		{
			startingTile.RemovePieceFromTile();

			if (!finishTile.TrySetPieceOnTile(movedPiece))
			{
				Debug.LogError($"Failed to place piece on a new tile.");
				return;
			}

			eventPieceMovedToAnotherTile?.Invoke(movedPiece, finishTile);
		}

		public void Undo()
		{
			finishTile.RemovePieceFromTile();

			if (!startingTile.TrySetPieceOnTile(movedPiece))
			{
				Debug.LogError($"Failed to place piece on the old tile.");
				return;
			}

			eventPieceMovedToAnotherTile?.Invoke(movedPiece, startingTile);
		}
	}
}