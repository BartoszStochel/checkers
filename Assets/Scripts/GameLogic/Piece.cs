namespace GameLogic
{
    public class Piece
    {
        public IPieceBehaviour Behaviour { get; private set; }
        public bool IsWhite { get; private set; }

        public Piece(IPieceBehaviour behaviour, bool isWhite)
		{
            Behaviour = behaviour;
            IsWhite = isWhite;
		}

        public void SetNewBehaviour(IPieceBehaviour newBehaviour)
		{
            Behaviour = newBehaviour;
		}
    }
}