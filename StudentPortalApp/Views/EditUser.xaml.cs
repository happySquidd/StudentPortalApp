using StudentPortalApp.Models;
using StudentPortalApp.Services;
using StudentPortalApp.Views;

namespace StudentPortalApp.Views;

public partial class EditUser : ContentPage
{
	public EditUser(string userName)
	{
		InitializeComponent();

		username.Text = userName;
	}

	private async void ChangeUsernameClicked(object sender, EventArgs e)
	{

	}

	private async void ChangePasswordClicked(object sender, EventArgs e)
	{

	}

	private async void SignOutClicked(object sender, EventArgs e)
	{
        var login = new Login();
        var LoginPage = new NavigationPage(login);
        Application.Current.Windows[0].Page = LoginPage;
    }
}