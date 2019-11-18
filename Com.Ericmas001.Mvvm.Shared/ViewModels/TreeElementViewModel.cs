using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Com.Ericmas001.Mvvm.Collections;
#pragma warning disable 1591

namespace Com.Ericmas001.Mvvm.ViewModels
{
    public abstract class TreeElementViewModel<T> : TreeElementViewModel where T : class
    {
        public new T Model { get; private set; }
        public virtual TreeElementViewModel<T> Init(TreeElementViewModel parent, T model)
        {
            base.Init(parent, model);
            Model = model;
            return this;
        }
    }
    public abstract class TreeElementViewModel : ViewModelBase
    {
        private bool _isExpanded;

        public virtual FontWeight FontWeight => FontWeights.Normal;
        public virtual FontFamily FontFamily => new FontFamily("Microsoft Sans Serif");
        public virtual FontStyle FontStyle => FontStyles.Normal;
        public virtual TextDecorationCollection FontDecoration => new TextDecorationCollection();
        public virtual Brush FontColor => Brushes.Black;

        public bool IsExpanded
        {
            get => _isExpanded;
            set => Set(ref _isExpanded, value);
        }

        public FastObservableCollection<TreeElementViewModel> Children { get; } = new FastObservableCollection<TreeElementViewModel>();

        public TreeElementViewModel Parent { get; private set; }
        public object Model { get; private set; }
        public abstract string Text { get; }
        public bool HasOnlyOneLeaf => NbLeaves == 1;
        public TreeElementViewModel FirstLeaf => TreeLeaves.FirstOrDefault();
        public bool HasChildren => Children.Any();
        public int NbChildren => Children.Count;
        public int NbLeaves => HasChildren ? TreeLeaves.Count() : 0;
        public bool HasGrandChildren => HasChildren && Children.Any(x => x.HasChildren);
        public bool CanExpand => HasChildren && !IsExpanded;
        public bool CanCollapse => HasChildren && IsExpanded;


        public virtual TreeElementViewModel Init(TreeElementViewModel parent, object model)
        {
            Parent = parent;
            Model = model;
            return this;
        }
        public void Expand()
        {
            IsExpanded = true;
        }
        public void ExpandAll()
        {
            Children.ToList().ForEach(x => x.ExpandAll());
            Expand();
        }
        public void ExpandWithParents()
        {
            Expand();
            Parent?.ExpandWithParents();
        }
        public void Collapse()
        {
            IsExpanded = false;
        }

        public void CollapseAll()
        {
            Collapse();
            Children.ToList().ForEach(x => x.CollapseAll());
        }
        public virtual IEnumerable<TreeElementViewModel> TreeLeaves
        {
            get { return HasChildren ? Children.SelectMany(x => x.TreeLeaves) : new[] { this }; }
        }

        public virtual void AddChildren(params TreeElementViewModel[] elems)
        {
            if (elems.Any())
                Children.AddItems(elems);
        }

    }
}
