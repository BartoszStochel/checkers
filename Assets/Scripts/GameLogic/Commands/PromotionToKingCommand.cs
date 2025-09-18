using System;

namespace GameLogic
{
	public class PromotionToKingCommand : ICommand
	{
		private Piece pieceToPromote;
		private Action<Piece> eventPieceBehaviourUpdated;

		private IPieceBehaviour oldBehaviour;

		public PromotionToKingCommand(Piece pieceToPromote, Action<Piece> eventPieceBehaviourUpdated)
		{
			this.pieceToPromote = pieceToPromote;
			this.eventPieceBehaviourUpdated = eventPieceBehaviourUpdated;
		}

		public void Execute()
		{
			oldBehaviour = pieceToPromote.Behaviour;
			pieceToPromote.SetNewBehaviour(new KingPieceBehaviour());
			eventPieceBehaviourUpdated?.Invoke(pieceToPromote);
		}

		public void Undo()
		{
			pieceToPromote.SetNewBehaviour(oldBehaviour);
			eventPieceBehaviourUpdated?.Invoke(pieceToPromote);
		}
	}
}