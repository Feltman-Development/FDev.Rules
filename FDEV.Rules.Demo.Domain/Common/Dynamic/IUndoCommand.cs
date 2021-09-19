namespace FDEV.Rules.Demo.Domain.Common.Dynamic
{
    public interface IUndoCommand
    {
        void Execute();
        void Inverse();
    }
}
