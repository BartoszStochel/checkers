using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using GameLogic;
using System.Collections.Generic;

namespace Presentation
{
    public class CheckersSceneManager : MonoBehaviour
    {
		[SerializeField]
		private int boardSizeX = 8;
		[SerializeField]
		private int boardSizeY = 8;
		[SerializeField]
		private int numberOfPiecesForOnePlayer = 12;
		[SerializeField]
		private int numbersOfMovesToUndo = 2;
		[SerializeField]
		private BasePlayerCreator firstPlayerCreator;
		[SerializeField]
		private BasePlayerCreator secondPlayerCreator;
		[SerializeField]
		private BoardPresentation boardPresentation;
		[SerializeField]
		private Text numberOfTurnsInHistoryText;

		private CheckersGame game;

		private void Start()
		{
			game = new CheckersGame(
				new Board(boardSizeX, boardSizeY),
				firstPlayerCreator.CreateNewPlayer(),
				secondPlayerCreator.CreateNewPlayer(),
				numberOfPiecesForOnePlayer);

			boardPresentation.InitializeBoard(game.Board);
			boardPresentation.EventRequestMovePieceToTile += OnEventRequestMovePieceToTile;

			game.EventHighlightPossibleMovingPieces += OnEventHighlightPossibleMovingPieces;
			game.EventHighlightPossibleAttackingPieces += OnEventHighlightPossibleAttackingPieces;
			game.EventPieceMovedToAnotherTile += OnEventPieceMovedToAnotherTile;
			game.EventPieceRemovedFromBoard += OnEventPieceRemovedFromBoard;
			game.EventGameIsFinished += OnEventGameIsFinished;
			game.EventPieceBehaviourUpdated += OnEventPieceBehaviourUpdated;
			game.EventTurnHistoryCountChanged += OnEventTurnHistoryCountChanged;

			game.StartNewGame();
		}

		private void Update()
		{
			EventSystem.current.SetSelectedGameObject(null);
		}

		public void UndoLastMoves()
		{
			game.UndoMoves(numbersOfMovesToUndo);
		}

		public void RestartGame()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		private void OnEventRequestMovePieceToTile(Piece piece, Tile targetTile)
		{
			game.RequestMovePieceToTile(piece, targetTile);
		}

		private void OnEventHighlightPossibleMovingPieces(List<PieceAndItsActionVectors> piecesWithMovements)
		{
			boardPresentation.InitiateMoveHighlight(piecesWithMovements);
		}

		private void OnEventHighlightPossibleAttackingPieces(List<PieceAndItsActionVectors> piecesWithAttacks)
		{
			boardPresentation.InitiateAttackHighlight(piecesWithAttacks);
		}

		private void OnEventPieceMovedToAnotherTile(Piece piece, Tile targetTile)
		{
			boardPresentation.MovePieceToTile(piece, targetTile);
		}

		private void OnEventPieceRemovedFromBoard(Piece pieceToRemove)
		{
			boardPresentation.RemovePieceFromBoard(pieceToRemove);
		}

		private void OnEventGameIsFinished()
		{
			boardPresentation.ResetMovementOptions();
		}

		private void OnEventPieceBehaviourUpdated(Piece updatedPiece)
		{
			boardPresentation.UpdatePiecePresentation(updatedPiece);
		}

		private void OnEventTurnHistoryCountChanged(int newValue)
		{
			numberOfTurnsInHistoryText.text = $"Number of turns in history: {newValue}";
		}

		private void OnDestroy()
		{
			boardPresentation.EventRequestMovePieceToTile -= OnEventRequestMovePieceToTile;
			game.EventHighlightPossibleMovingPieces -= OnEventHighlightPossibleMovingPieces;
			game.EventHighlightPossibleAttackingPieces -= OnEventHighlightPossibleAttackingPieces;
			game.EventPieceMovedToAnotherTile -= OnEventPieceMovedToAnotherTile;
			game.EventPieceRemovedFromBoard -= OnEventPieceRemovedFromBoard;
			game.EventGameIsFinished -= OnEventGameIsFinished;
			game.EventPieceBehaviourUpdated -= OnEventPieceBehaviourUpdated;
			game.EventTurnHistoryCountChanged -= OnEventTurnHistoryCountChanged;
		}
	}
}