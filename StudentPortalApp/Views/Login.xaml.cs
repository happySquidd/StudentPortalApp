namespace StudentPortalApp.Views;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
	}

	private void LoginClicked(object sender, EventArgs e)
	{
		// create a new root window which is the terms view page
		var terms = new TermsPage();
		var TermsPage = new NavigationPage(terms);
		Application.Current.Windows[0].Page = TermsPage;
	}
}