using Com.Ericmas001.DependencyInjection.Registrants;

namespace Com.Ericmas001.MvvmCore
{
    public class MvvmCoreRegistrant : AbstractRegistrant
    {
        public WindowCreator WindowCreator { get; } = new WindowCreator();
        protected override void RegisterEverything()
        {
            RegisterInstance<IWindowCreator>(WindowCreator);
        }
    }
}
