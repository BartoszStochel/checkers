using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class CheckersGame
    {
        public IBoard Board { get; private set; }
        public IPlayer WhitePlayer { get; private set; }
        public IPlayer BlackPlayer { get; private set; }

        public event Action<List<PieceAndItsActionVectors>> EventHighlightPossibleAttackingPieces;
        public event Action<List<PieceAndItsActionVectors>> EventHighlightPossibleMovingPieces;
        public event Action<Piece, Tile> EventPieceMovedToAnotherTile;
        public event Action<Piece> EventPieceRemovedFromBoard;
        public event Action EventGameIsFinished;
        public event Action<Piece> EventPieceBehaviourUpdated;
        public event Action<int> EventTurnHistoryCountChanged;

        private IActionVectorsCalculator attackVectorsCalculator;
        private IActionVectorsCalculator movementVectorsCalculator;
        private int numberOfPiecesForOnePlayer;
        private IPlayer currentPlayer;
        private List<PieceAndItsActionVectors> legalMovesForCurrentPlayer;
        private Stack<PlayerTurnData> turnsHistory;
        private Stack<ICommand> actionsPerformedInThisTurn;

        public CheckersGame(IBoard newBoard, IPlayer firstPlayer, IPlayer secondPlayer, int newNumberOfPiecesForOnePlayer)
		{
            Board = newBoard;
            WhitePlayer = firstPlayer;
            BlackPlayer = secondPlayer;

            attackVectorsCalculator = new AttackVectorsCalculator(Board);
            movementVectorsCalculator = new MovementVectorsCalculator(Board);
            numberOfPiecesForOnePlayer = newNumberOfPiecesForOnePlayer;
            turnsHistory = new Stack<PlayerTurnData>();
            actionsPerformedInThisTurn = new Stack<ICommand>();
        }

        public void StartNewGame()
        {
            ShufflePlayerOrder(true);

            WhitePlayer.SetNewPieces(GetNewSetOfPieces(numberOfPiecesForOnePlayer, true));
            BlackPlayer.SetNewPieces(GetNewSetOfPieces(numberOfPiecesForOnePlayer, false));

            bool whiteShoudBeAtTheBottomBeacuseThereAreTwoSamePlayers = WhitePlayer.IsHumanPlayer == BlackPlayer.IsHumanPlayer;
            Board.PlacePiecesOnProperTilesAndSetMovementDirection(WhitePlayer.Pieces, !whiteShoudBeAtTheBottomBeacuseThereAreTwoSamePlayers && !WhitePlayer.IsHumanPlayer);
            Board.PlacePiecesOnProperTilesAndSetMovementDirection(BlackPlayer.Pieces, whiteShoudBeAtTheBottomBeacuseThereAreTwoSamePlayers || !BlackPlayer.IsHumanPlayer);

            currentPlayer = WhitePlayer;
            LetCurrentPlayerPerformTheirTurn();
        }

        public void UndoMoves(int numbersOfMovesToUndo)
		{
            while (actionsPerformedInThisTurn.TryPop(out var action))
            {
                action.Undo();
            }

            for (int i = 0; i < numbersOfMovesToUndo; i++)
			{
                if (!turnsHistory.TryPop(out var lastTurn))
				{
                    break;
				}

                lastTurn.UndoEveryAction();
                EventTurnHistoryCountChanged?.Invoke(turnsHistory.Count);
                SwitchCurrentPlayer();
            }

            LetCurrentPlayerPerformTheirTurn();
        }

        public void RequestMovePieceToTile(Piece piece, Tile targetTile)
		{
            for (int i = 0; i < legalMovesForCurrentPlayer.Count; i++)
			{
                if (piece != legalMovesForCurrentPlayer[i].Piece)
				{
                    continue;
				}

                for (int j = 0; j < legalMovesForCurrentPlayer[i].ActionVectors.Count; j++)
                {
                    for (int k = 0; k < legalMovesForCurrentPlayer[i].ActionVectors[j].LandingTargets.Count; k++)
                    {
                        if (targetTile != legalMovesForCurrentPlayer[i].ActionVectors[j].LandingTargets[k])
                        {
                            continue;
                        }

                        // Move is legal. Proceed.
                        PerformPieceMovement(piece, targetTile, legalMovesForCurrentPlayer[i].ActionVectors[j].AttackTarget);
                        return;
                    }
                }
            }

            Debug.LogError($"Illegal try to move piece to tile with position: {Board.GetTilePositionOnBoard(targetTile)}");
        }

        private void PerformPieceMovement(Piece pieceToMove, Tile targetTile, Piece attackTarget)
		{
            HandleMovementCommand(pieceToMove, targetTile);

            List<PieceAndItsActionVectors> nextPossibleAttacksOfPiece = null;

            if (attackTarget != null)
            {
                HandleRemovePieceFromBoardCommand(attackTarget);
                nextPossibleAttacksOfPiece = attackVectorsCalculator.GetPiecesThatCanActNow(new List<Piece> { pieceToMove });
            }

            HandlePromoteManToKingCommand(pieceToMove);

            // Try to perform next attack by the same piece
            if (nextPossibleAttacksOfPiece != null && nextPossibleAttacksOfPiece.Count > 0)
			{
                TryToPerformAction(nextPossibleAttacksOfPiece, EventHighlightPossibleAttackingPieces);
            }
            // Or move on to turn of next player
			else
			{
                turnsHistory.Push(new PlayerTurnData(actionsPerformedInThisTurn));
                EventTurnHistoryCountChanged?.Invoke(turnsHistory.Count);
                actionsPerformedInThisTurn.Clear();

                SwitchCurrentPlayer();
                LetCurrentPlayerPerformTheirTurn();
            }
        }

        private void SwitchCurrentPlayer()
		{
            if (currentPlayer == WhitePlayer)
            {
                currentPlayer = BlackPlayer;
            }
            else if (currentPlayer == BlackPlayer)
            {
                currentPlayer = WhitePlayer;
            }
        }

        private void HandleMovementCommand(Piece pieceToMove, Tile targetTile)
		{
            var oldPieceTilePosition = Board.GetPiecePositionOnBoard(pieceToMove);
            var oldTile = Board.Tiles[oldPieceTilePosition.x, oldPieceTilePosition.y];

            HandleCommand(new MovementCommand(pieceToMove, oldTile, targetTile, EventPieceMovedToAnotherTile));
        }

        private void HandleRemovePieceFromBoardCommand(Piece attackTarget)
		{
            var attackedTilePosition = Board.GetPiecePositionOnBoard(attackTarget);
            var attackedTile = Board.Tiles[attackedTilePosition.x, attackedTilePosition.y];
            var ownerOfPiece = attackTarget.IsWhite ? WhitePlayer : BlackPlayer;

            HandleCommand(new RemovePieceFromBoardCommand(attackTarget, attackedTile, ownerOfPiece, EventPieceRemovedFromBoard, Board.InvokeEventPieceSpawnedOnPosition));
        }

        private void HandlePromoteManToKingCommand(Piece man)
		{
            if (!Board.ManHasReachedOppositeEdgeOfTheBoard(man))
            {
                return;
            }

            HandleCommand(new PromotionToKingCommand(man, EventPieceBehaviourUpdated));
        }

        private void HandleCommand(ICommand command)
		{
            command.Execute();
            actionsPerformedInThisTurn.Push(command);
        }

        private void LetCurrentPlayerPerformTheirTurn()
		{
            if (TryToPerformAction(attackVectorsCalculator.GetPlayerPiecesThatCanActNow(currentPlayer), EventHighlightPossibleAttackingPieces) ||
                TryToPerformAction(movementVectorsCalculator.GetPlayerPiecesThatCanActNow(currentPlayer), EventHighlightPossibleMovingPieces))
            {
                return;
			}

            FinishGame(currentPlayer);
        }

        private bool TryToPerformAction(List<PieceAndItsActionVectors> playerPiecesThatCanAct, Action<List<PieceAndItsActionVectors>> highlightAction)
		{
            if (playerPiecesThatCanAct.Count == 0)
            {
                return false;
            }

            legalMovesForCurrentPlayer = playerPiecesThatCanAct;

            if (currentPlayer.IsHumanPlayer)
            {
                highlightAction?.Invoke(playerPiecesThatCanAct);
            }
            else
            {
                var actionData = currentPlayer.GetActionData(playerPiecesThatCanAct);
                PerformPieceMovement(actionData.pieceToPerformAction, actionData.landingTile, actionData.attackTarget);
            }

            return true;
        }

        private void FinishGame(IPlayer loser)
		{
            string winnerName = loser == WhitePlayer ? "Black player" : "White player";
            string loserName = loser == WhitePlayer ? "White player" : "Black player";

            if (loser.Pieces.Count == 0)
            {
                Debug.Log($"{winnerName} wins! {loserName} has no pieces left on the board.");
            }
            else
            {
                Debug.Log($"{winnerName} wins! {loserName} has some pieces left, but they can't move anywhere.");
            }

            EventGameIsFinished?.Invoke();
        }

        private List<Piece> GetNewSetOfPieces(int numberOfPieces, bool areWhite)
		{
            var pieces = new List<Piece>(numberOfPieces);

            for (int i = 0; i < numberOfPieces; i++)
			{
                pieces.Add(new Piece(new ManPieceBehaviour(), areWhite));
			}

            return pieces;
		}

        public void ShufflePlayerOrder(bool shouldItBeRandom)
		{
            if (!shouldItBeRandom || UnityEngine.Random.Range(0, 2) == 0)
			{
                var firstPlayer = WhitePlayer;
                WhitePlayer = BlackPlayer;
                BlackPlayer = firstPlayer;
			}
		}
    }
}   