using NUnit.Framework;
using GameLogic;

namespace Tests
{
    public class CheckersGameTests
    {
        [Test]
        public void PlayersHaveProperNumberAndColorOfPiecesAfterGameStart()
        {
            int numberOfPieces = 7;
            CheckersGame game = new CheckersGame(new Board(8, 8), new HumanPlayer(), new HumanPlayer(), numberOfPieces);
            game.StartNewGame();

            Assert.AreEqual(game.WhitePlayer.Pieces.Count, numberOfPieces);
            Assert.AreEqual(game.BlackPlayer.Pieces.Count, numberOfPieces);

            Assert.True(game.WhitePlayer.Pieces[0].IsWhite);
            Assert.True(game.WhitePlayer.Pieces[numberOfPieces - 1].IsWhite);
            Assert.False(game.BlackPlayer.Pieces[0].IsWhite);
            Assert.False(game.BlackPlayer.Pieces[numberOfPieces - 1].IsWhite);
        }

        [Test]
        public void ShufflingPlayersWorksProperly()
        {
            HumanPlayer playerA = new HumanPlayer();
            HumanPlayer playerB = new HumanPlayer();
            CheckersGame game = new CheckersGame(new Board(8, 8), playerA, playerB, 1);

            Assert.AreSame(playerA, game.WhitePlayer);
            Assert.AreSame(playerB, game.BlackPlayer);

            game.ShufflePlayerOrder(false);

            Assert.AreSame(playerA, game.BlackPlayer);
            Assert.AreSame(playerB, game.WhitePlayer);
        }
    }
}