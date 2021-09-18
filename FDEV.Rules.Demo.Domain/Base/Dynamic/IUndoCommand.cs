namespace FDEV.Rules.Demo.Domain.Base.Dynamic
{
    public interface IUndoCommand
    {
        void Execute();
        void Inverse();
    }
}
