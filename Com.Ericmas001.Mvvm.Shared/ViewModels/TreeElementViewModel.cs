using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Com.Ericmas001.Mvvm.Collections;
#pragma warning disable 1591

namespace Com.Ericmas001.Mvvm.ViewModels
{
    public abstract class TreeElementViewModel<TBaseElementViewModel,TModel> : AbstractTreeElementViewModel<TBaseElementViewModel>
        where TBaseElementViewModel : AbstractTreeElementViewModel<TBaseElementViewModel> 
        where TModel : class
    {
        public new TModel Model { get; private set; }
        public virtual void Init(TBaseElementViewModel parent, TModel model)
        {
            base.Init(parent, model);
            Model = model;
        }
    }
    public abstract class TreeElementViewModel<T> : TreeElementViewModel where T : class
    {
        public new T Model { get; private set; }
        public virtual void Init(TreeElementViewModel parent, T model)
        {
            base.Init(parent, model);
            Model = model;
        }
    }
    public abstract class TreeElementViewModel : AbstractTreeElementViewModel<TreeElementViewModel>
    {

    }
    public abstract class AbstractTreeElementViewModel<TBaseElementViewModel> : SuperBasicTreeElementViewModel where TBaseElementViewModel : AbstractTreeElementViewModel<TBaseElementViewModel>
    {
        public FastObservableCollection<TBaseElementViewModel> Children { get; } = new FastObservableCollection<TBaseElementViewModel>();

        public TBaseElementViewModel Parent { get; private set; }
        public bool HasOnlyOneLeaf => NbLeaves == 1;
        public AbstractTreeElementViewModel<TBaseElementViewModel> FirstLeaf => TreeLeaves.FirstOrDefault();
        public bool HasChildren => Children.Any();
        public int NbChildren => Children.Count;
        public int NbLeaves => HasChildren ? TreeLeaves.Count() : 0;
        public bool HasGrandChildren => HasChildren && Children.Any(x => x.HasChildren);
        public bool CanExpand => HasChildren && !IsExpanded;
        public bool CanCollapse => HasChildren && IsExpanded;


        public virtual void Init(TBaseElementViewModel parent, object model)
        {
            base.Init(model);
            Parent = parent;
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
        public void CollapseAll()
        {
            Collapse();
            Children.ToList().ForEach(x => x.CollapseAll());
        }
        public virtual IEnumerable<AbstractTreeElementViewModel<TBaseElementViewModel>> TreeLeaves
        {
            get { return HasChildren ? Children.SelectMany(x => x.TreeLeaves) : new [] { this }; }
        }

        public virtual void AddChildren(params TBaseElementViewModel[] elems)
        {
            if (elems.Any())
                Children.AddItems(elems);
        }

    }
    public abstract class SuperBasicTreeElementViewModel : ViewModelBase
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
        public object Model { get; private set; }
        public abstract string Text { get; }


        public void Init(object model)
        {
            Model = model;
        }
        public void Expand()
        {
            IsExpanded = true;
        }
        public void Collapse()
        {
            IsExpanded = false;
        }
    }
}
