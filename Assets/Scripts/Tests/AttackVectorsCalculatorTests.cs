using NUnit.Framework;
using GameLogic;
using UnityEngine;
using System.Collections.Generic;

namespace Tests
{
    public class AttackVectorsCalculatorTests
    {
        private IBoard board;
        private IActionVectorsCalculator calculator;

        [SetUp]
        public void Setup()
        {
            board = new Board(8, 8);
            calculator = new AttackVectorsCalculator(board);
        }

        [Test]
        public void ManIsAbleToAttackAdjacentEnemyToTheRight()
        {
            var attacker = new Piece(new ManPieceBehaviour(), true);
            var defender = new Piece(new ManPieceBehaviour(), false);

            board.PlacePieceOnCustomPosition(attacker, new Vector2Int(0, 0), false);
            board.PlacePieceOnCustomPosition(defender, new Vector2Int(1, 1), true);

            var attackVectors = calculator.GetActionVectorsOfPiece(attacker);

            Assert.NotNull(attackVectors);
            Assert.AreEqual(1, attackVectors.Count);
            Assert.AreSame(defender, attackVectors[0].AttackTarget);
            Assert.AreEqual(1, attackVectors[0].LandingTargets.Count);
            Assert.AreSame(board.Tiles[2, 2], attackVectors[0].LandingTargets[0]);
        }

        [Test]
        public void BlackManIsAbleToAttackAdjacentEnemyToTheRight()
        {
            var attacker = new Piece(new ManPieceBehaviour(), false);
            var defender = new Piece(new ManPieceBehaviour(), true);

            board.PlacePieceOnCustomPosition(attacker, new Vector2Int(0, 0), false);
            board.PlacePieceOnCustomPosition(defender, new Vector2Int(1, 1), true);

            var attackVectors = calculator.GetActionVectorsOfPiece(attacker);

            Assert.NotNull(attackVectors);
            Assert.AreEqual(1, attackVectors.Count);
            Assert.AreSame(defender, attackVectors[0].AttackTarget);
            Assert.AreEqual(1, attackVectors[0].LandingTargets.Count);
            Assert.AreSame(board.Tiles[2, 2], attackVectors[0].LandingTargets[0]);
        }

        [Test]
        public void ManIsAbleToAttackAdjacentEnemyToTheLeft()
        {
            var attacker = new Piece(new ManPieceBehaviour(), true);
            var defender = new Piece(new ManPieceBehaviour(), false);

            board.PlacePieceOnCustomPosition(attacker, new Vector2Int(2, 0), false);
            board.PlacePieceOnCustomPosition(defender, new Vector2Int(1, 1), true);

            var attackVectors = calculator.GetActionVectorsOfPiece(attacker);

            Assert.NotNull(attackVectors);
            Assert.AreEqual(1, attackVectors.Count);
            Assert.AreSame(defender, attackVectors[0].AttackTarget);
            Assert.AreEqual(1, attackVectors[0].LandingTargets.Count);
            Assert.AreSame(board.Tiles[0, 2], attackVectors[0].LandingTargets[0]);
        }

        [Test]
        public void ManIsUnableToAttackAdjacentEnemyThatIsAtTheEdgeOfTheBoard()
        {
            var attacker = new Piece(new ManPieceBehaviour(), true);
            var defender = new Piece(new ManPieceBehaviour(), false);

            board.PlacePieceOnCustomPosition(attacker, new Vector2Int(1, 1), false);
            board.PlacePieceOnCustomPosition(defender, new Vector2Int(0, 2), true);

            var attackVectors = calculator.GetActionVectorsOfPiece(attacker);

            Assert.NotNull(attackVectors);
            Assert.AreEqual(0, attackVectors.Count);
        }

        [Test]
        public void ManIsUnableToAttackAdjacentFriend()
        {
            var attacker = new Piece(new ManPieceBehaviour(), true);
            var defender = new Piece(new ManPieceBehaviour(), true);

            board.PlacePieceOnCustomPosition(attacker, new Vector2Int(0, 0), false);
            board.PlacePieceOnCustomPosition(defender, new Vector2Int(1, 1), false);

            var attackVectors = calculator.GetActionVectorsOfPiece(attacker);

            Assert.NotNull(attackVectors);
            Assert.AreEqual(0, attackVectors.Count);
        }

        [Test]
        public void ManIsUnableToAttackAdjacentEnemyThatHasAnotherEnemyBehind()
        {
            var attacker = new Piece(new ManPieceBehaviour(), true);
            var defender = new Piece(new ManPieceBehaviour(), false);
            var behindDefender = new Piece(new ManPieceBehaviour(), false);

            board.PlacePieceOnCustomPosition(attacker, new Vector2Int(0, 0), false);
            board.PlacePieceOnCustomPosition(defender, new Vector2Int(1, 1), true);
            board.PlacePieceOnCustomPosition(behindDefender, new Vector2Int(2, 2), true);

            var attackVectors = calculator.GetActionVectorsOfPiece(attacker);

            Assert.NotNull(attackVectors);
            Assert.AreEqual(0, attackVectors.Count);
        }

        [Test]
        public void ManIsAbleToAttackAdjacentEnemyWhenStartingFromTop()
        {
            var attacker = new Piece(new ManPieceBehaviour(), true);
            var defender = new Piece(new ManPieceBehaviour(), false);

            board.PlacePieceOnCustomPosition(attacker, new Vector2Int(2, 2), true);
            board.PlacePieceOnCustomPosition(defender, new Vector2Int(3, 1), false);

            var attackVectors = calculator.GetActionVectorsOfPiece(attacker);

            Assert.NotNull(attackVectors);
            Assert.AreEqual(1, attackVectors.Count);
            Assert.AreSame(defender, attackVectors[0].AttackTarget);
            Assert.AreEqual(1, attackVectors[0].LandingTargets.Count);
            Assert.AreSame(board.Tiles[4, 0], attackVectors[0].LandingTargets[0]);
        }

        [Test]
        public void KingIsAbleToAttackEnemyAFewTilesAway()
        {
            var attacker = new Piece(new KingPieceBehaviour(), true);
            var defender = new Piece(new ManPieceBehaviour(), false);

            board.PlacePieceOnCustomPosition(attacker, new Vector2Int(0, 0), false);
            board.PlacePieceOnCustomPosition(defender, new Vector2Int(4, 4), true);

            var attackVectors = calculator.GetActionVectorsOfPiece(attacker);

            Assert.NotNull(attackVectors);
            Assert.AreEqual(1, attackVectors.Count);
            Assert.AreSame(defender, attackVectors[0].AttackTarget);
            Assert.AreEqual(3, attackVectors[0].LandingTargets.Count);
            Assert.AreSame(board.Tiles[5, 5], attackVectors[0].LandingTargets[0]);
            Assert.AreSame(board.Tiles[6, 6], attackVectors[0].LandingTargets[1]);
            Assert.AreSame(board.Tiles[7, 7], attackVectors[0].LandingTargets[2]);
        }

        [Test]
        public void KingIsAbleToAttackEnemyAndLandingTilesAreLimitedByNextEnemy()
        {
            var attacker = new Piece(new KingPieceBehaviour(), true);
            var defender = new Piece(new ManPieceBehaviour(), false);
            var behindDefender = new Piece(new ManPieceBehaviour(), false);

            board.PlacePieceOnCustomPosition(attacker, new Vector2Int(0, 0), false);
            board.PlacePieceOnCustomPosition(defender, new Vector2Int(4, 4), true);
            board.PlacePieceOnCustomPosition(behindDefender, new Vector2Int(6, 6), true);

            var attackVectors = calculator.GetActionVectorsOfPiece(attacker);

            Assert.NotNull(attackVectors);
            Assert.AreEqual(1, attackVectors.Count);
            Assert.AreSame(defender, attackVectors[0].AttackTarget);
            Assert.AreEqual(1, attackVectors[0].LandingTargets.Count);
            Assert.AreSame(board.Tiles[5, 5], attackVectors[0].LandingTargets[0]);
        }

        [Test]
        public void ManIsAbleToAttackFourAdjacdentEnemies()
        {
            var attacker = new Piece(new ManPieceBehaviour(), true);
            var defenderA = new Piece(new ManPieceBehaviour(), false);
            var defenderB = new Piece(new ManPieceBehaviour(), false);
            var defenderC = new Piece(new ManPieceBehaviour(), false);
            var defenderD = new Piece(new ManPieceBehaviour(), false);

            board.PlacePieceOnCustomPosition(attacker, new Vector2Int(4, 4), false);
            board.PlacePieceOnCustomPosition(defenderA, new Vector2Int(5, 5), true);
            board.PlacePieceOnCustomPosition(defenderB, new Vector2Int(3, 5), true);
            board.PlacePieceOnCustomPosition(defenderC, new Vector2Int(5, 3), true);
            board.PlacePieceOnCustomPosition(defenderD, new Vector2Int(3, 3), true);

            var attackVectors = calculator.GetActionVectorsOfPiece(attacker);

            Assert.NotNull(attackVectors);
            Assert.AreEqual(4, attackVectors.Count);
            Assert.AreEqual(1, attackVectors[0].LandingTargets.Count);
            Assert.AreEqual(1, attackVectors[1].LandingTargets.Count);
            Assert.AreEqual(1, attackVectors[2].LandingTargets.Count);
            Assert.AreEqual(1, attackVectors[3].LandingTargets.Count);
        }

        [Test]
        public void KingIsAbleToAttackFourAdjacdentEnemies()
        {
            var attacker = new Piece(new KingPieceBehaviour(), true);
            var defenderA = new Piece(new ManPieceBehaviour(), false);
            var defenderB = new Piece(new ManPieceBehaviour(), false);
            var defenderC = new Piece(new ManPieceBehaviour(), false);
            var defenderD = new Piece(new ManPieceBehaviour(), false);

            board.PlacePieceOnCustomPosition(attacker, new Vector2Int(4, 4), false);
            board.PlacePieceOnCustomPosition(defenderA, new Vector2Int(5, 5), true);
            board.PlacePieceOnCustomPosition(defenderB, new Vector2Int(3, 5), true);
            board.PlacePieceOnCustomPosition(defenderC, new Vector2Int(5, 3), true);
            board.PlacePieceOnCustomPosition(defenderD, new Vector2Int(3, 3), true);

            var attackVectors = calculator.GetActionVectorsOfPiece(attacker);

            Assert.NotNull(attackVectors);
            Assert.AreEqual(4, attackVectors.Count);
            Assert.AreEqual(2, attackVectors[0].LandingTargets.Count);
            Assert.AreEqual(2, attackVectors[1].LandingTargets.Count);
            Assert.AreEqual(2, attackVectors[2].LandingTargets.Count);
            Assert.AreEqual(3, attackVectors[3].LandingTargets.Count);
        }

        [Test]
        public void NoManCanAttackInTheBeginningOfStandardPlay()
        {
            var pieces = new List<Piece>();
            var enemyPieces = new List<Piece>();

            for (int i = 0; i < 12; i++)
            {
                pieces.Add(new Piece(new ManPieceBehaviour(), true));
                enemyPieces.Add(new Piece(new ManPieceBehaviour(), false));
            }

            board.PlacePiecesOnProperTilesAndSetMovementDirection(pieces, false);
            board.PlacePiecesOnProperTilesAndSetMovementDirection(enemyPieces, true);

            var piecesThatCanMove = calculator.GetPiecesThatCanActNow(pieces);

            Assert.NotNull(piecesThatCanMove);
            Assert.AreEqual(0, piecesThatCanMove.Count);
        }
    }
}