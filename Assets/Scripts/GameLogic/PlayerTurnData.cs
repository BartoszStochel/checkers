using System.Collections.Generic;

namespace GameLogic
{
    public class PlayerTurnData
    {
        public Stack<ICommand> ActionsPerformedInThisTurn { get; private set; }

        public PlayerTurnData(Stack<ICommand> actionsPerformedInThisTurn)
		{
            ActionsPerformedInThisTurn = new Stack<ICommand>(new Stack<ICommand>(actionsPerformedInThisTurn));
		}

        public void UndoEveryAction()
        {
            while (ActionsPerformedInThisTurn.TryPop(out var lastAction))
			{
                lastAction.Undo();
			}
		}
    }
}