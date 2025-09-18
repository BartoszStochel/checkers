using NUnit.Framework;
using UnityEngine;
using GameLogic;
using System.Collections.Generic;

namespace Tests
{
    public class MovementVectorsCalculatorTests
    {
        private IBoard board;
        private IActionVectorsCalculator calculator;

        [SetUp]
        public void Setup()
        {
            board = new Board(8, 8);
            calculator = new MovementVectorsCalculator(board);
        }

        [Test]
        public void ManCanMoveInTwoWaysUp()
        {
            var piece = new Piece(new ManPieceBehaviour(), false);

            board.PlacePieceOnCustomPosition(piece, new Vector2Int(2, 0), false);

            var movementVectors = calculator.GetActionVectorsOfPiece(piece);

            Assert.NotNull(movementVectors);
            Assert.AreEqual(2, movementVectors.Count);
            Assert.AreEqual(1, movementVectors[0].LandingTargets.Count);
            Assert.AreEqual(1, movementVectors[1].LandingTargets.Count);
            Assert.AreSame(board.Tiles[1, 1], movementVectors[0].LandingTargets[0]);
            Assert.AreSame(board.Tiles[3, 1], movementVectors[1].LandingTargets[0]);
        }

        [Test]
        public void ManCanMoveInTwoWaysDown()
        {
            var piece = new Piece(new ManPieceBehaviour(), false);

            board.PlacePieceOnCustomPosition(piece, new Vector2Int(1, 7), true);

            var movementVectors = calculator.GetActionVectorsOfPiece(piece);

            Assert.NotNull(movementVectors);
            Assert.AreEqual(2, movementVectors.Count);
            Assert.AreEqual(1, movementVectors[0].LandingTargets.Count);
            Assert.AreEqual(1, movementVectors[1].LandingTargets.Count);
            Assert.AreSame(board.Tiles[0, 6], movementVectors[0].LandingTargets[0]);
            Assert.AreSame(board.Tiles[2, 6], movementVectors[1].LandingTargets[0]);
        }

        [Test]
        public void ManCanNotMoveUpWhenNearTopBorder()
        {
            var piece = new Piece(new ManPieceBehaviour(), false);

            board.PlacePieceOnCustomPosition(piece, new Vector2Int(1, 7), false);

            var movementVectors = calculator.GetActionVectorsOfPiece(piece);

            Assert.NotNull(movementVectors);
            Assert.AreEqual(0, movementVectors.Count);
        }

        [Test]
        public void ManCanNotMoveDownWhenNearBottomBorder()
        {
            var piece = new Piece(new ManPieceBehaviour(), false);

            board.PlacePieceOnCustomPosition(piece, new Vector2Int(2, 0), true);

            var movementVectors = calculator.GetActionVectorsOfPiece(piece);

            Assert.NotNull(movementVectors);
            Assert.AreEqual(0, movementVectors.Count);
        }

        [Test]
        public void ManCanMoveOneWayWhenNearSideBorder()
        {
            var piece = new Piece(new ManPieceBehaviour(), false);

            board.PlacePieceOnCustomPosition(piece, new Vector2Int(0, 0), false);

            var movementVectors = calculator.GetActionVectorsOfPiece(piece);

            Assert.NotNull(movementVectors);
            Assert.AreEqual(1, movementVectors.Count);
            Assert.AreEqual(1, movementVectors[0].LandingTargets.Count);
            Assert.AreSame(board.Tiles[1, 1], movementVectors[0].LandingTargets[0]);
        }

        [Test]
        public void ManIsBlockedByAnotherPiece()
        {
            var piece = new Piece(new ManPieceBehaviour(), false);
            var blockingPiece = new Piece(new ManPieceBehaviour(), false);

            board.PlacePieceOnCustomPosition(piece, new Vector2Int(2, 0), false);
            board.PlacePieceOnCustomPosition(blockingPiece, new Vector2Int(1, 1), false);

            var movementVectors = calculator.GetActionVectorsOfPiece(piece);

            Assert.NotNull(movementVectors);
            Assert.AreEqual(1, movementVectors.Count);
            Assert.AreEqual(1, movementVectors[0].LandingTargets.Count);
            Assert.AreSame(board.Tiles[3, 1], movementVectors[0].LandingTargets[0]);
        }

        [Test]
        public void KingCanMoveMultipleTiles()
        {
            var piece = new Piece(new KingPieceBehaviour(), false);

            board.PlacePieceOnCustomPosition(piece, new Vector2Int(0, 0), false);

            var movementVectors = calculator.GetActionVectorsOfPiece(piece);

            Assert.NotNull(movementVectors);
            Assert.AreEqual(1, movementVectors.Count);
            Assert.AreEqual(7, movementVectors[0].LandingTargets.Count);
            Assert.AreSame(board.Tiles[7, 7], movementVectors[0].LandingTargets[6]);
        }

        [Test]
        public void KingCanMoveFourWays()
        {
            var piece = new Piece(new KingPieceBehaviour(), false);

            board.PlacePieceOnCustomPosition(piece, new Vector2Int(3, 3), false);

            var movementVectors = calculator.GetActionVectorsOfPiece(piece);

            Assert.NotNull(movementVectors);
            Assert.AreEqual(4, movementVectors.Count);
            Assert.AreEqual(3, movementVectors[0].LandingTargets.Count);
            Assert.AreEqual(4, movementVectors[1].LandingTargets.Count);
            Assert.AreEqual(3, movementVectors[2].LandingTargets.Count);
            Assert.AreEqual(3, movementVectors[3].LandingTargets.Count);
        }

        [Test]
        public void FourMansCanMoveInTheBeginningOfStandardPlay()
        {
            var pieces = new List<Piece>();

            for (int i = 0; i < 12; i++)
			{
                pieces.Add(new Piece(new ManPieceBehaviour(), true));
			}

            board.PlacePiecesOnProperTilesAndSetMovementDirection(pieces, false);

            var piecesThatCanMove = calculator.GetPiecesThatCanActNow(pieces);

            Assert.NotNull(piecesThatCanMove);
            Assert.AreEqual(4, piecesThatCanMove.Count);
            Assert.AreSame(pieces[8], piecesThatCanMove[0].Piece);
            Assert.AreSame(pieces[9], piecesThatCanMove[1].Piece);
            Assert.AreSame(pieces[10], piecesThatCanMove[2].Piece);
            Assert.AreSame(pieces[11], piecesThatCanMove[3].Piece);
        }
    }
}