using StudentPortalApp.Views;

namespace StudentPortalApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var gadgetList = new GadgetList();
            var tabPage = new NavigationPage(gadgetList);
            return new Window(tabPage);
        }
    }
}