using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Com.Ericmas001.MvvmCore
{
    public interface IViewManager
    {
        TWindow CreateWindow<TWindow, TViewModel>() where TWindow : Window where TViewModel : ViewModelBase;
        TViewModel CreateViewModel<TViewModel>() where TViewModel : ViewModelBase;
        TWindow CreateWindow<TWindow>(object viewModel) where TWindow : Window;
    }

    public class ViewManager : IViewManager
    {
        public IHost Host { get; set; }

        public TWindow CreateWindow<TWindow>() where TWindow : Window
        {
            return Host.Services.GetRequiredService<TWindow>();
        }

        public TWindow CreateWindow<TWindow, TViewModel>() where TWindow : Window where TViewModel : ViewModelBase
        {
            var window = Host.Services.GetRequiredService<TWindow>();
            window.DataContext = Host.Services.GetRequiredService<TViewModel>();
            return window;
        }

        public TViewModel CreateViewModel<TViewModel>() where TViewModel : ViewModelBase
        {
            return Host.Services.GetRequiredService<TViewModel>();
        }

        public TWindow CreateWindow<TWindow>(object viewModel) where TWindow : Window
        {
            var window = Host.Services.GetRequiredService<TWindow>();
            window.DataContext = viewModel;
            return window;
        }
    }
}
