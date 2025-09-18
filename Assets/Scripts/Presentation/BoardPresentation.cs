using UnityEngine;
using GameLogic;
using System;
using System.Collections.Generic;

namespace Presentation
{
    public class BoardPresentation : MonoBehaviour
    {
		[SerializeField]
		private TilePresentation darkTilePrefab;
		[SerializeField]
		private TilePresentation lightTilePrefab;
		[SerializeField]
		private PiecePresentation whiteManPiece;
		[SerializeField]
		private PiecePresentation blackManPiece;
		[SerializeField]
		private PiecePresentation whiteKingPiece;
		[SerializeField]
		private PiecePresentation blackKingPiece;

		public event Action<Piece, Tile> EventRequestMovePieceToTile;

		private IBoard board;
		private TilePresentation[,] tilePresentations;
		private Dictionary<Piece, PiecePresentation> piecePresentations = new Dictionary<Piece, PiecePresentation>();

		private List<PieceAndItsActionVectors> currentMovementOptions;
		private PieceAndItsActionVectors currentlySelectedPieceToMove;
		private Action<TilePresentation> currentHighlightAction;
		private List<TilePresentation> currentlyHighlightedTiles = new List<TilePresentation>();

		public void InitializeBoard(IBoard boardToRepresent)
		{
			board = boardToRepresent;
			tilePresentations = new TilePresentation[board.SizeX, board.SizeY];

			for (int x = 0; x < board.SizeX; x++)
			{
                for (int y = 0; y < board.SizeY; y++)
				{
					tilePresentations[x, y] = CreateTile(x, y, board.SizeX, board.SizeY);
				}
			}

			board.EventPieceSpawnedOnPosition += OnEventPieceSpawnedOnPosition;
		}

		public void InitiateMoveHighlight(List<PieceAndItsActionVectors> piecesWithActions)
		{
			currentMovementOptions = piecesWithActions;
			currentlySelectedPieceToMove = null;
			currentHighlightAction = HighlightMove;
			HighlightRoots();
		}

		public void InitiateAttackHighlight(List<PieceAndItsActionVectors> piecesWithActions)
		{
			currentMovementOptions = piecesWithActions;
			currentlySelectedPieceToMove = null;
			currentHighlightAction = HighlightAttack;
			HighlightRoots();
		}

		public void RemovePieceFromBoard(Piece piece)
		{
			Destroy(piecePresentations[piece].gameObject);
			piecePresentations.Remove(piece);
		}

		public void MovePieceToTile(Piece piece, Tile tile)
		{
			var tilePosition = board.GetTilePositionOnBoard(tile);
			piecePresentations[piece].MoveToPosition(tilePresentations[tilePosition.x, tilePosition.y].RectTransform.anchoredPosition);
		}

		private void HighlightRoots()
		{
			DeactivateCurrentHighlights();

			for (int i = 0; i < currentMovementOptions.Count; i++)
			{
				HighlightTile(board.GetPiecePositionOnBoard(currentMovementOptions[i].Piece), currentHighlightAction);
			}
		}

		public void ResetMovementOptions()
		{
			currentMovementOptions = null;
			currentlySelectedPieceToMove = null;
		}

		public void UpdatePiecePresentation(Piece pieceToUpdate)
		{
			var oldGameObject = piecePresentations[pieceToUpdate];
			Destroy(oldGameObject.gameObject);

			var piecePosition = board.GetPiecePositionOnBoard(pieceToUpdate);
			InstantiateAndSetupPiecePresentation(pieceToUpdate, piecePosition.x, piecePosition.y);
		}

		private void HighlightChildren(List<ActionVectorAndItsTarget> childrenToHighlight)
		{
			DeactivateCurrentHighlights();

			for (int i = 0; i < childrenToHighlight.Count; i++)
			{
				if (childrenToHighlight[i].AttackTarget != null)
				{
					HighlightTile(board.GetPiecePositionOnBoard(childrenToHighlight[i].AttackTarget), HighlightAttackTarget);
				}

				for (int j = 0; j < childrenToHighlight[i].LandingTargets.Count; j++)
				{
					HighlightTile(board.GetTilePositionOnBoard(childrenToHighlight[i].LandingTargets[j]), currentHighlightAction);
				}
			}
		}

		private void HighlightTile(Vector2Int position, Action<TilePresentation> highlightAction)
		{
			var tileToHighlight = tilePresentations[position.x, position.y];
			highlightAction?.Invoke(tileToHighlight);
			currentlyHighlightedTiles.Add(tileToHighlight);
		}

		private void HighlightMove(TilePresentation tile)
		{
			tile.ActivateMoveHighlight();
		}

		private void HighlightAttack(TilePresentation tile)
		{
			tile.ActivateAttackHighlight();
		}

		private void HighlightAttackTarget(TilePresentation tile)
		{
			tile.ActivateAttackTargetHighlight();
		}

		private void DeactivateCurrentHighlights()
		{
			for (int i = 0; i < currentlyHighlightedTiles.Count; i++)
			{
				currentlyHighlightedTiles[i].DeactivateHighlights();
			}

			currentlyHighlightedTiles.Clear();
		}

		private void OnEventPieceSpawnedOnPosition(Piece spawnedPiece, int x, int y)
		{
			InstantiateAndSetupPiecePresentation(spawnedPiece, x, y);
		}

		private void InstantiateAndSetupPiecePresentation(Piece spawnedPiece, int x, int y)
		{
			var piece = Instantiate(GetProperPrefabForPiece(spawnedPiece), transform);
			piece.RectTransform.anchoredPosition = tilePresentations[x, y].RectTransform.anchoredPosition;
			piecePresentations[spawnedPiece] = piece;
		}

		private PiecePresentation GetProperPrefabForPiece(Piece piece)
		{
			if (piece.IsWhite)
			{
				if (piece.Behaviour is ManPieceBehaviour)
				{
					return whiteManPiece;
				}
				else
				{
					return whiteKingPiece;
				}
			}
			else
			{
				if (piece.Behaviour is ManPieceBehaviour)
				{
					return blackManPiece;
				}
				else
				{
					return blackKingPiece;
				}
			}
		}

		private TilePresentation CreateTile(int x, int y, int boardSizeX, int boardSizeY)
		{
			TilePresentation tilePrefabToInstantiate;

			if ((x + y) % 2 == 0)
			{
				tilePrefabToInstantiate = darkTilePrefab;
			}
			else
			{
				tilePrefabToInstantiate = lightTilePrefab;
			}

			var tile = Instantiate(tilePrefabToInstantiate, transform);

			var tileSizeX = tile.RectTransform.sizeDelta.x;
			var tileSizeY = tile.RectTransform.sizeDelta.y;

			tile.RectTransform.anchoredPosition = new Vector2(
				-(boardSizeX - 1f) / 2f * tileSizeX + x * tileSizeX,
				-(boardSizeY - 1f) / 2f * tileSizeY + y * tileSizeY);

			tile.DeactivateHighlights();
			tile.SetOnButtonClickedAction(() => OnTileClicked(x, y));

			return tile;
		}

		private void OnTileClicked(int x, int y)
		{
			if (currentMovementOptions == null || currentMovementOptions.Count == 0)
			{
				return;
			}

			if (currentlySelectedPieceToMove == null)
			{
				for (int i = 0; i < currentMovementOptions.Count; i++)
				{
					if (board.Tiles[x, y].PieceOnTile == currentMovementOptions[i].Piece)
					{
						currentlySelectedPieceToMove = currentMovementOptions[i];
						HighlightChildren(currentMovementOptions[i].ActionVectors);

						return;
					}
				}
			}
			else
			{
				for (int i = 0; i < currentlySelectedPieceToMove.ActionVectors.Count; i++)
				{
					for (int j = 0; j < currentlySelectedPieceToMove.ActionVectors[i].LandingTargets.Count; j++)
					{
						if (board.Tiles[x, y] == currentlySelectedPieceToMove.ActionVectors[i].LandingTargets[j])
						{
							DeactivateCurrentHighlights();
							EventRequestMovePieceToTile?.Invoke(currentlySelectedPieceToMove.Piece, currentlySelectedPieceToMove.ActionVectors[i].LandingTargets[j]);
							currentlySelectedPieceToMove = null;

							return;
						}
					}
				}

				currentlySelectedPieceToMove = null;
				HighlightRoots();
			}
		}

		private void OnDestroy()
		{
			board.EventPieceSpawnedOnPosition -= OnEventPieceSpawnedOnPosition;
		}
	}
}