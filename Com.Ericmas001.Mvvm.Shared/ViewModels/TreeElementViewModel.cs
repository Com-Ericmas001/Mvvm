using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Com.Ericmas001.Mvvm.Collections;
#pragma warning disable 1591

namespace Com.Ericmas001.Mvvm.ViewModels
{
    public abstract class TreeElementViewModel<T> : TreeElementViewModel where T : class
    {
        public new T Model { get; private set; }
        public virtual void Init(TreeElementViewModel parent, T model)
        {
            base.Init(parent, model);
            Model = model;
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
            set
            {
                var isChanging = value != _isExpanded;
                if (isChanging)
                {
                    if (value)
                        OnBeforeExpand();
                    else
                        OnBeforeCollapse();
                }
                Set(ref _isExpanded, value);
                if (isChanging)
                {
                    // ReSharper disable once AssignmentIsFullyDiscarded
                    _ = value ? OnAfterExpand() : OnAfterCollapse();
                }
            }
        }

        protected virtual async Task OnAfterCollapse()
        {
            await Task.CompletedTask;
        }

        protected virtual async Task OnAfterExpand()
        {
            await Task.CompletedTask;
        }

        protected virtual void OnBeforeExpand()
        {
        }
        protected virtual void OnBeforeCollapse()
        {
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


        public virtual void Init(TreeElementViewModel parent, object model)
        {
            Parent = parent;
            Model = model;
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
