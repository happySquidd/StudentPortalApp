using StudentPortalApp.Services;
using StudentPortalApp.Models;

namespace StudentPortalApp.Views;

public partial class Register : ContentPage
{
	public Register()
	{
		InitializeComponent();
	}

	private async void RegisterClicked(object sender, EventArgs e)
	{
		User user = new User();
		
		// input validation
		if (string.IsNullOrWhiteSpace(username.Text))
		{
			usernameEmpty.IsVisible = true;
			return;
		}
		else
		{
			usernameEmpty.IsVisible = false;
		}
		if (string.IsNullOrWhiteSpace(password.Text))
		{
			passwordEmpty.IsVisible = true;
			return;
		}
		else
		{
			passwordEmpty.IsVisible = false;
		}
		if (!await DatabaseService.AddUser(username.Text, password.Text))
		{
			usernameTaken.IsVisible = true;
			return;
		}
		else
		{
			// created user successfully
			usernameTaken.IsVisible = false;
			// get the new user to assign id
			user = await DatabaseService.GetUser(username.Text);
		}
		// assign Id
		var terms = new TermsPage(user.Id, user.UserName);
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