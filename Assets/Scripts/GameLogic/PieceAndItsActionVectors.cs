using System.Collections.Generic;

namespace GameLogic
{
    public class PieceAndItsActionVectors
    {
        public Piece Piece { get; private set; }
        public List<ActionVectorAndItsTarget> ActionVectors { get; private set; }

        public PieceAndItsActionVectors(Piece piece, List<ActionVectorAndItsTarget> actionVectors)
		{
            Piece = piece;
            ActionVectors = actionVectors;
		}
    }
}