using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace FDEV.Rules.Demo.Domain.Base.Dynamic
{
    public abstract class DynamicPropertyCollectionBase : ObservableCollection<DynamicPropertyBase>, IDynamicPropertyCollection //where TItem : DynamicPropertyBase
    {
        protected DynamicPropertyCollectionBase(IDynamicPropertyCollection collection, bool isSelected)
        {
            _collection = collection;
            _isSelected = isSelected;
            _itemType = collection.GetType().GetGenericArguments()[0];
            //TODO: Fix with another weak event
            //CollectionChangedEventManager.AddHandler(collection, ModelCollection_CollectionChanged);

            foreach (var item in collection)
            {
                //TODO: Setup weak listening with 
                //DynamicPropertyBase toAdd = new DynamicPropertyBase(item as INotifyPropertyChanged);
                //toAdd.PropertyChanged += ChildItem_PropertyChanged;
                //toAdd.ParentCollection = this;
                //Add(toAdd);
            }
        }

        private void ChildItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "IsSelected") return;
            var item = sender as DynamicPropertyBase;

            if (_selectedItem != item) _selectedItem = item;
            else if (_selectedItem == item)_selectedItem = null;
            
            OnPropertyChanged(new PropertyChangedEventArgs("SelectedItem"));
        }

        //TODO: Rethink (simplify) collectionchanged setup
        private void ModelCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add: //TODO: Rethink (simplify) collectionchanged setup
                    //DynamicPropertyBase toAdd = new DynamicPropertyBase(e.NewItems[0] as INotifyPropertyChanged);
                    //toAdd.PropertyChanged += ChildItem_PropertyChanged;
                    //toAdd.ParentCollection = this;
                    //Insert(e.NewStartingIndex, toAdd);
                    OnPropertyChanged(new PropertyChangedEventArgs("SelectedItem"));
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveAt(e.OldStartingIndex);
                    OnPropertyChanged(new PropertyChangedEventArgs("SelectedItem"));
                    break;
                case NotifyCollectionChangedAction.Move:
                    Move(e.OldStartingIndex, e.NewStartingIndex);
                    break;
            }
        }
        
        public DelegateCommand AddCommand => new(AddCommand_Execute, AddCommand_CanExecute);

        public DelegateCommand RemoveCommand => new(RemoveCommand_Execute, RemoveCommand_CanExecute);

        public DelegateCommand MoveUpCommand => new(MoveUpCommand_Execute, MoveUpCommand_CanExecute);

        public DelegateCommand MoveDownCommand => new(MoveDownCommand_Execute, MoveDownCommand_CanExecute);

        private bool AddCommand_CanExecute() => true;

        private void AddCommand_Execute()
        {
            var toAdd = Activator.CreateInstance(_itemType) as INotifyPropertyChanged;
            //TODO: Change undo implementation> Simplify slightly
            //UndoManager.Do(new ChangeCollectionCommand(_collection, toAdd, _collection.Count, ChangeCollectionMode.Insert);
        }

        private bool RemoveCommand_CanExecute() => GetSelectionInfo(out var selectionCount, out var selectedIndex);

        private void RemoveCommand_Execute()
        {
            if (!GetSelectionInfo(out var selectionCount, out var selectedIndex)) return;
            //TODO: Change undo implementation > Simplify slightly
            //UndoManager.Do(new ChangeCollectionCommand(_collection, _collection[selectedIndex] as INotifyPropertyChanged, selectedIndex,                     ChangeCollectionMode.Remove));
        }

        private bool MoveUpCommand_CanExecute() => GetSelectionInfo(out var selectionCount, out var selectedIndex) && selectedIndex != 0;

        private void MoveUpCommand_Execute()
        {
            if (!GetSelectionInfo(out var selectionCount, out var selectedIndex)) return;
            if (selectedIndex == 0) return;

            //TODO: Change undo implementation > Simplify slightly
            //UndoManager.Do(new ChangeCollectionCommand(_collection, _collection[selectedIndex] as INotifyPropertyChanged, selectedIndex, ChangeCollectionMode.MoveUp));
        }

        private bool MoveDownCommand_CanExecute() => GetSelectionInfo(out var selectionCount, out var selectedIndex) && selectedIndex != (_collection.Count - 1);

        private void MoveDownCommand_Execute()
        {
            if (!GetSelectionInfo(out var selectionCount, out var selectedIndex)) return;
            if (selectedIndex == (_collection.Count - 1)) return;
           
            //TODO: Change undo implementation > Simplify slightly
            //UndoManager.Do(new ChangeCollectionCommand(_collection, _collection[selectedIndex] as INotifyPropertyChanged, selectedIndex, ChangeCollectionMode.MoveDown));
        }

        private bool GetSelectionInfo(out int selectionCount, out int selectedIndex)
        {
            selectionCount = 0;
            selectedIndex = -1;
            var firstAlreadyFound = false;
            for (var i = 0; i < Count; i++)
            {
                if (!firstAlreadyFound)
                {
                    firstAlreadyFound = true;
                    selectedIndex = i;
                }
                selectionCount++;
            }
            return selectionCount == 1;
        }

        private bool _isSelected;
        private readonly IDynamicPropertyCollection _collection;
        private DynamicPropertyBase _selectedItem;
        private readonly Type _itemType;


        public ObservableCollection<DynamicPropertyBase> Collection { get; set; }

        //TODO: Extend the base functionality of the dynamic collection
    }
}