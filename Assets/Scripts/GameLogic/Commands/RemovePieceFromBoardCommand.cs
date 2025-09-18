using System;

namespace GameLogic
{
	public class RemovePieceFromBoardCommand : ICommand
	{
		private Piece pieceToRemove;
		private Tile tileWithPiece;
		private IPlayer ownerOfPiece;
		private Action<Piece> eventPieceRemovedFromBoard;
		private Action<Piece, Tile> eventPieceSpawnedOnTile;

		private int indexInPlayersPieces;

		public RemovePieceFromBoardCommand(Piece pieceToRemove, Tile tileWithPiece, IPlayer ownerOfPiece, Action<Piece> eventPieceRemovedFromBoard, Action<Piece, Tile> eventPieceSpawnedOnTile)
		{
			this.pieceToRemove = pieceToRemove;
			this.tileWithPiece = tileWithPiece;
			this.ownerOfPiece = ownerOfPiece;
			this.eventPieceSpawnedOnTile = eventPieceSpawnedOnTile;
			this.eventPieceRemovedFromBoard = eventPieceRemovedFromBoard;
		}

		public void Execute()
		{
			tileWithPiece.RemovePieceFromTile();
			indexInPlayersPieces = ownerOfPiece.Pieces.IndexOf(pieceToRemove);
			ownerOfPiece.Pieces.Remove(pieceToRemove);
			eventPieceRemovedFromBoard?.Invoke(pieceToRemove);
		}

		public void Undo()
		{
			tileWithPiece.TrySetPieceOnTile(pieceToRemove);
			ownerOfPiece.Pieces.Insert(indexInPlayersPieces, pieceToRemove);
			eventPieceSpawnedOnTile?.Invoke(pieceToRemove, tileWithPiece);
		}
	}
}