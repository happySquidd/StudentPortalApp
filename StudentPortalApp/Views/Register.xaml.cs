namespace StudentPortalApp.Views;

public partial class Register : ContentPage
{
	public Register()
	{
		InitializeComponent();
	}

	private void RegisterClicked(object sender, EventArgs e)
	{
        var terms = new TermsPage();
        var TermsPage = new NavigationPage(terms);
        Application.Current.Windows[0].Page = TermsPage;
    }
	private void LoginClicked(object sender, EventArgs e)
	{
		var login = new Login();
		var LoginPage = new NavigationPage(login);
		Application.Current.Windows[0].Page = LoginPage;
	}
}