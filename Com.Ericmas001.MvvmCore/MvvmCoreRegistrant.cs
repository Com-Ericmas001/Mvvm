using Com.Ericmas001.DependencyInjection.Registrants;

namespace Com.Ericmas001.MvvmCore
{
    public class MvvmCoreRegistrant : AbstractRegistrant
    {
        public ViewManager ViewManager { get; } = new ViewManager();
        protected override void RegisterEverything()
        {
            RegisterInstance<IViewManager>(ViewManager);
        }
    }
}
