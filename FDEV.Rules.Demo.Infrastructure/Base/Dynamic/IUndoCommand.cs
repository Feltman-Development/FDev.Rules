namespace FSites.Core.Domain.Dynamic
{
    public interface IUndoCommand
    {
        void Execute();
        void Inverse();
    }
}
