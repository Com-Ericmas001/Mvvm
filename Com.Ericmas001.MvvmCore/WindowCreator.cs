using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Com.Ericmas001.MvvmCore
{
    public interface IWindowCreator
    {
        TWindow CreateWindow<TWindow>() where TWindow : Window;
    }

    public class WindowCreator : IWindowCreator
    {
        public IHost Host { get; set; }

        public TWindow CreateWindow<TWindow>() where TWindow : Window
        {
            return Host.Services.GetRequiredService<TWindow>();
        }
    }
}
