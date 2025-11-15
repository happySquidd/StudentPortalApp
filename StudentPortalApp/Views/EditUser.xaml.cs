using StudentPortalApp.Models;
using StudentPortalApp.Services;
using StudentPortalApp.Views;

namespace StudentPortalApp.Views;

public partial class EditUser : ContentPage
{
	private string Username { get; set; }
	public EditUser(string userName)
	{
		InitializeComponent();

		Username = userName;
	}

	private async void ChangeUsernameClicked(object sender, EventArgs e)
	{
		User user = await DatabaseService.GetUser(Username);
		await Navigation.PushAsync(new ChangeName(user));
	}

	private async void ChangePasswordClicked(object sender, EventArgs e)
	{
        User user = await DatabaseService.GetUser(Username);
        await Navigation.PushAsync(new ChangePassword(user));
	}

	private void SignOutClicked(object sender, EventArgs e)
	{
        var login = new Login();
        var LoginPage = new NavigationPage(login);
        Application.Current.Windows[0].Page = LoginPage;
    }
}