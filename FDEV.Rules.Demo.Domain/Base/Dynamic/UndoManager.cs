using System.Collections.Generic;

namespace FDEV.Rules.Demo.Domain.Base.Dynamic
{
    //TODO: IS SET TO NOT BUILD FOR NOW!
    public class UndoManager : DynamicPropertyBase
    {
        public UndoManager()
        {
            _undoStack = new Stack<IUndoCommand>();
            _redoStack = new Stack<IUndoCommand>();
        }

        [NotifyOnChange(nameof(ChangeCounter))]
        public bool UnsavedChanges => ChangeCounter != 0;

        private int ChangeCounter { get; set; }
        
        public void AccountForChanges() => ChangeCounter = 0;

        public void Do(IUndoCommand command)
        {
            _redoStack.Clear();
            _undoStack.Push(command);
            command.Execute();
            ChangeCounter++;
        }

        public DelegateCommand UndoCommand => new(Undo, CanUndo);
        public bool CanUndo() => _undoStack.Count != 0;
        public void Undo()
        {
            if (!CanUndo()) return;

            var command = _undoStack.Pop();
            _redoStack.Push(command);
            command.Inverse();
            ChangeCounter--;
        }
        private readonly Stack<IUndoCommand> _undoStack;

        public DelegateCommand RedoCommand => new(Redo, CanRedo);
        public bool CanRedo() => _redoStack.Count != 0;
        public void Redo()
        {
            if (!CanRedo()) return;

            var command = _redoStack.Pop();
            _undoStack.Push(command);
            command.Execute();
            ChangeCounter++;
        }
        private readonly Stack<IUndoCommand> _redoStack;
    }
}
