using StudentPortalApp.Services;
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
            var login = new Login();
            var LoginPage = new NavigationPage(login);
            return new Window(LoginPage);
        }
    }
}