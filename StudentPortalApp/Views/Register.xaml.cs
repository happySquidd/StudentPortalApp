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
		if (string.IsNullOrWhiteSpace(username.Text) || string.IsNullOrWhiteSpace(password.Text))
		{
			await DisplayAlert(
				title: "Error",
				message: "Please enter your username and password",
				cancel: "OK");
			return;
		}
		if (!await DatabaseService.AddUser(username.Text, password.Text))
		{
			await DisplayAlert(
				title: "Error",
				message: "This username is taken please try another",
				cancel: "OK");
			return;
		}
		else
		{
			Console.WriteLine("Getting user ID");
			// get the new user to assign id
			user = await DatabaseService.GetUser(username.Text);
			Console.WriteLine($"username and password = {user.UserName}:{user.Password}");
		}
		// assign Id
		var terms = new TermsPage(user.Id);
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