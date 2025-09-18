namespace GameLogic
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}