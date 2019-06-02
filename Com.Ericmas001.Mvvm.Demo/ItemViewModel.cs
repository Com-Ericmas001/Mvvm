using Com.Ericmas001.Mvvm.Collections;

namespace Com.Ericmas001.Mvvm.Demo
{
    public class ItemViewModel
    {
        public string Name { get; }
        public FastObservableCollection<ItemViewModel> Children { get; } = new FastObservableCollection<ItemViewModel>();

        public ItemViewModel(string name, params ItemViewModel[] initialChildren)
        {
            Name = name;
            Children.AddItems(initialChildren);
        }
    }
}
