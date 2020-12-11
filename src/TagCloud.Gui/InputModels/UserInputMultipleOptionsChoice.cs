using System.Collections.Generic;
using System.Linq;

namespace TagCloud.Gui.InputModels
{
    public class UserInputMultipleOptionsChoice<T> : UserInputChoiceBase<T>
    {
        private readonly Dictionary<string, bool> selectionByNames;
        private ISet<T>? bound;

        public UserInputMultipleOptionsChoice(string description, UserInputSelectorItem<T>[] available)
            : base(description, available)
        {
            selectionByNames = Available.ToDictionary(x => x.Name, _ => false);
        }

        public IEnumerable<T> Selected => Available.Where(x => IsSelected(x.Name)).Select(x => x.Value);
        public bool IsSelected(string name) => selectionByNames[name];

        public void SetSelection(string name, bool isSelected)
        {
            if (IsSelected(name) != isSelected)
            {
                selectionByNames[name] = isSelected;
                UpdateBoundCollection(name, isSelected);
            }
        }

        public void BindSelectionTo(ISet<T> target)
        {
            bound = target;
            foreach (var item in Selected)
                target.Add(item);
        }

        private void UpdateBoundCollection(string updatedName, bool isSelected)
        {
            if (bound == null) return;
            var updatedItem = Available.Single(x => x.Name == updatedName).Value;
            if (isSelected) bound.Add(updatedItem!);
            else bound.Remove(updatedItem!);
        }
    }
}