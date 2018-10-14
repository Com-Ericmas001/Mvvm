using System.Linq;
using System.Windows.Input;
using Com.Ericmas001.Mvvm.Collections;

namespace Com.Ericmas001.Mvvm.Demo
{
    public class MainViewModel : ViewModelBase
    {
        public FastObservableCollection<ItemViewModel> Items => new FastObservableCollection<ItemViewModel>
        {
            new ItemViewModel("Original", 
                new ItemViewModel("First Child",
                    new ItemViewModel("First GrandChild"),
                    new ItemViewModel("Second GrandChild")),
                new ItemViewModel("Second Child"))
        };

        public ItemViewModel Selected
        {
            get => _selected;
            set => Set( ref _selected, value);
        }

        private ICommand _selectRootCommand;

        public ICommand SelectRootCommand => _selectRootCommand  =_selectRootCommand ?? new RelayCommand(() => Selected = Items.First());

        private ItemViewModel _selected;
    }
}
