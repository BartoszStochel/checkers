using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
	public class Board : IBoard
	{
		public int SizeX { get; private set; }
		public int SizeY { get; private set; }
		public Tile[,] Tiles { get; private set; }

		public event Action<Piece, int, int> EventPieceSpawnedOnPosition;

		public Board(int sizeX, int sizeY)
		{
			SizeX = sizeX;
			SizeY = sizeY;

			Tiles = new Tile[SizeX, SizeY];

			for (int x = 0; x < SizeX; x++)
			{
				for (int y = 0; y < SizeY; y++)
				{
					bool canPieceBePlacedOnTile = (x + y) % 2 == 0;
					Tiles[x, y] = new Tile(canPieceBePlacedOnTile);
				}
			}
		}

		public void InvokeEventPieceSpawnedOnPosition(Piece piece, Tile tile)
		{
			var tilePosition = GetTilePositionOnBoard(tile);
			EventPieceSpawnedOnPosition?.Invoke(piece, tilePosition.x, tilePosition.y);
		}

		public void PlacePiecesOnProperTilesAndSetMovementDirection(List<Piece> pieces, bool shouldPlaceOnTopOfTheBoard)
		{
			int numberOfPlacedPieces = 0;

			for (int y = 0; y < SizeY; y++)
			{
				for (int x = 0; x < SizeX; x++)
				{
					if (numberOfPlacedPieces >= pieces.Count)
					{
						return;
					}

					Piece piece = pieces[numberOfPlacedPieces];

					int tilePositionX = x;
					int tilePositionY = y;

					if (shouldPlaceOnTopOfTheBoard)
					{
						tilePositionX = SizeX - x - 1;
						tilePositionY = SizeY - y - 1;
					}

					if (Tiles[tilePositionX, tilePositionY].TrySetPieceOnTile(piece))
					{
						piece.Behaviour.SetMovementDirection(shouldPlaceOnTopOfTheBoard);
						EventPieceSpawnedOnPosition?.Invoke(piece, tilePositionX, tilePositionY);
						numberOfPlacedPieces++;
					}
				}
			}
		}

		public void PlacePieceOnCustomPosition(Piece piece, Vector2Int position, bool hasStartedGameOnTopOfTheBoard)
		{
			if (Tiles[position.x, position.y].TrySetPieceOnTile(piece))
			{
				piece.Behaviour.SetMovementDirection(hasStartedGameOnTopOfTheBoard);
				EventPieceSpawnedOnPosition?.Invoke(piece, position.x, position.y);
			}
		}

		public Vector2Int GetPiecePositionOnBoard(Piece piece)
		{
			for (int x = 0; x < SizeX; x++)
			{
				for (int y = 0; y < SizeY; y++)
				{
					if (Tiles[x, y].PieceOnTile == piece)
					{
						return new Vector2Int(x, y);
					}
				}
			}

			return new Vector2Int(-1, -1);
		}

		public Vector2Int GetTilePositionOnBoard(Tile tile)
		{
			for (int x = 0; x < SizeX; x++)
			{
				for (int y = 0; y < SizeY; y++)
				{
					if (Tiles[x, y] == tile)
					{
						return new Vector2Int(x, y);
					}
				}
			}

			return new Vector2Int(-1, -1);
		}

		public bool ManHasReachedOppositeEdgeOfTheBoard(Piece piece)
		{
			var piecePosition = GetPiecePositionOnBoard(piece);

			for (int i = 0; i < piece.Behaviour.MovementVectors.Count; i++)
			{
				var nextPositionAfterMove = piecePosition + piece.Behaviour.MovementVectors[i];

				if (nextPositionAfterMove.y >= 0 && nextPositionAfterMove.y < SizeY)
				{
					return false;
				}
			}

			return true;
		}
	}
}