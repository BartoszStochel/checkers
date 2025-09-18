using System.Collections.Generic;
using NUnit.Framework;
using GameLogic;
using UnityEngine;

namespace Tests
{
    public class BoardTests
    {
        [Test]
        public void RectangleBoardIsInitializedWithProperSize()
		{
            BoardIsInitializedWithProperSize(10, 5);
        }

        [Test]
        public void SquareBoardIsInitializedWithProperSize()
		{
            BoardIsInitializedWithProperSize(8, 8);
        }

        private void BoardIsInitializedWithProperSize(int boardSizeX, int boardSizeY)
		{
            IBoard board = new Board(boardSizeX, boardSizeY);

            Assert.NotNull(board);
            Assert.AreEqual(board.SizeX, boardSizeX);
            Assert.AreEqual(board.SizeY, boardSizeY);

            Assert.NotNull(board.Tiles);
            Assert.AreEqual(board.Tiles.GetLength(0), boardSizeX);
            Assert.AreEqual(board.Tiles.GetLength(1), boardSizeY);
        }

        [Test]
        public void TilesAreInitializedProperly()
        {
            IBoard board = new Board(8, 8);

            Assert.True(board.Tiles[0, 0].CanPieceBePlacedHere);
            Assert.True(board.Tiles[7, 7].CanPieceBePlacedHere);
            Assert.True(board.Tiles[0, 0].CanPieceBePlacedHere);
            Assert.True(board.Tiles[6, 6].CanPieceBePlacedHere);
            Assert.True(board.Tiles[0, 6].CanPieceBePlacedHere);
            Assert.True(board.Tiles[6, 0].CanPieceBePlacedHere);

            Assert.False(board.Tiles[0, 7].CanPieceBePlacedHere);
            Assert.False(board.Tiles[7, 0].CanPieceBePlacedHere);
            Assert.False(board.Tiles[0, 1].CanPieceBePlacedHere);
            Assert.False(board.Tiles[1, 0].CanPieceBePlacedHere);
        }

        [Test]
        public void PiecesArePlacedCorrectlyFromTheBottom()
        {
            IBoard board = new Board(8, 8);
            List<Piece> pieces = new List<Piece>();

            for (int i = 0; i < 12; i++)
			{
                pieces.Add(new Piece(new ManPieceBehaviour(), false));
			}

            board.PlacePiecesOnProperTilesAndSetMovementDirection(pieces, false);

            Assert.NotNull(board.Tiles[0, 0].PieceOnTile);
            Assert.True(board.Tiles[0, 0].PieceOnTile.Behaviour.MovementVectors[0].y == 1);
            Assert.Null(board.Tiles[1, 0].PieceOnTile);

            Assert.NotNull(board.Tiles[6, 2].PieceOnTile);
            Assert.True(board.Tiles[6, 2].PieceOnTile.Behaviour.MovementVectors[0].y == 1);
            Assert.Null(board.Tiles[7, 2].PieceOnTile);

            Assert.Null(board.Tiles[1, 3].PieceOnTile);
        }

        [Test]
        public void PiecesArePlacedCorrectlyFromTheTop()
        {
            IBoard board = new Board(8, 8);
            List<Piece> pieces = new List<Piece>();

            for (int i = 0; i < 12; i++)
			{
                pieces.Add(new Piece(new ManPieceBehaviour(), false));
			}

            board.PlacePiecesOnProperTilesAndSetMovementDirection(pieces, true);

            Assert.NotNull(board.Tiles[7, 7].PieceOnTile);
            Assert.True(board.Tiles[7, 7].PieceOnTile.Behaviour.MovementVectors[0].y == -1);
            Assert.Null(board.Tiles[6, 7].PieceOnTile);

            Assert.NotNull(board.Tiles[1, 5].PieceOnTile);
            Assert.True(board.Tiles[1, 5].PieceOnTile.Behaviour.MovementVectors[0].y == -1);
            Assert.Null(board.Tiles[0, 5].PieceOnTile);

            Assert.Null(board.Tiles[6, 4].PieceOnTile);
        }

        [Test]
        public void EventPieceSpawnedOnPositionIsInvokedCorrectly()
        {
            IBoard board = new Board(8, 8);
            List<Piece> pieces = new List<Piece>();
            int howManyPiecesShouldBeSpawned = 10;

            for (int i = 0; i < howManyPiecesShouldBeSpawned; i++)
			{
                pieces.Add(new Piece(new ManPieceBehaviour(), false));
            }

            int howManyTimesEventWasInvoked = 0;

            board.EventPieceSpawnedOnPosition += (piece, x, y) => howManyTimesEventWasInvoked++;

            board.PlacePiecesOnProperTilesAndSetMovementDirection(pieces, true);

            Assert.AreEqual(howManyTimesEventWasInvoked, howManyPiecesShouldBeSpawned);
        }

        [Test]
        public void GetPositionReturnsCorrectPiecePosition()
        {
            IBoard board = new Board(8, 8);
            List<Piece> piecesBottom = new List<Piece>();
            List<Piece> piecesTop = new List<Piece>();
            int howManyPiecesShouldBeSpawned = 12;

            for (int i = 0; i < howManyPiecesShouldBeSpawned; i++)
			{
                piecesBottom.Add(new Piece(new ManPieceBehaviour(), false));
                piecesTop.Add(new Piece(new ManPieceBehaviour(), false));
            }

            board.PlacePiecesOnProperTilesAndSetMovementDirection(piecesBottom, false);
            board.PlacePiecesOnProperTilesAndSetMovementDirection(piecesTop, true);

            Assert.AreEqual(board.GetPiecePositionOnBoard(piecesBottom[0]), Vector2Int.zero);
            Assert.AreEqual(board.GetPiecePositionOnBoard(piecesBottom[3]), new Vector2Int(6, 0));
            Assert.AreEqual(board.GetPiecePositionOnBoard(piecesBottom[4]), new Vector2Int(1, 1));
            Assert.AreEqual(board.GetPiecePositionOnBoard(piecesBottom[11]), new Vector2Int(6, 2));

            Assert.AreEqual(board.GetPiecePositionOnBoard(piecesTop[0]), new Vector2Int(7, 7));
            Assert.AreEqual(board.GetPiecePositionOnBoard(piecesTop[3]), new Vector2Int(1, 7));
            Assert.AreEqual(board.GetPiecePositionOnBoard(piecesTop[7]), new Vector2Int(0, 6));
            Assert.AreEqual(board.GetPiecePositionOnBoard(piecesTop[11]), new Vector2Int(1, 5));
        }

        [Test]
        public void CustomPiecePlacementWorksProperly()
        {
            IBoard board = new Board(8, 8);

            var pieceA = new Piece(new ManPieceBehaviour(), false);
            var pieceB = new Piece(new ManPieceBehaviour(), false);
            var pieceC = new Piece(new ManPieceBehaviour(), false);
            var pieceD = new Piece(new ManPieceBehaviour(), false);
            var pieceE = new Piece(new ManPieceBehaviour(), false);

            var pieceAPosition = new Vector2Int(0, 0);
            var pieceBPosition = new Vector2Int(7, 1);
            var pieceCPosition = new Vector2Int(6, 0);
            var pieceDPosition = new Vector2Int(7, 7);
            var pieceEPosition = new Vector2Int(3, 3);

            board.PlacePieceOnCustomPosition(pieceA, pieceAPosition, false);
            board.PlacePieceOnCustomPosition(pieceB, pieceBPosition, true);
            board.PlacePieceOnCustomPosition(pieceC, pieceCPosition, true);
            board.PlacePieceOnCustomPosition(pieceD, pieceDPosition, false);
            board.PlacePieceOnCustomPosition(pieceE, pieceEPosition, false);

            Assert.AreEqual(board.GetPiecePositionOnBoard(pieceA), pieceAPosition);
            Assert.AreEqual(board.GetPiecePositionOnBoard(pieceB), pieceBPosition);
            Assert.AreEqual(board.GetPiecePositionOnBoard(pieceC), pieceCPosition);
            Assert.AreEqual(board.GetPiecePositionOnBoard(pieceD), pieceDPosition);
            Assert.AreEqual(board.GetPiecePositionOnBoard(pieceE), pieceEPosition);
        }
    }
}