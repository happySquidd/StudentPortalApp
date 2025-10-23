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
            var TermsPage = new TermsPage(); // new name is ViewTerms
            var TermPage = new NavigationPage(TermsPage);
            return new Window(TermPage);
        }
    }
}