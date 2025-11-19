using StudentPortalApp.Models;
using StudentPortalApp.Services;

namespace StudentPortalApp.Views;

public partial class ForgotPassword : ContentPage
{
	public ForgotPassword()
	{
		InitializeComponent();

		passwordLabel.IsVisible = false;
		passwordText.IsVisible = false;
		userEmpty.IsVisible = false;
		userNotFound.IsVisible = false;
	}

	private async void GetPasswordClicked(object sender, EventArgs e)
	{
		User user = new User();
		
		// input validation
		if (string.IsNullOrWhiteSpace(username.Text)) 
		{
			userEmpty.IsVisible = true;
			return;
		}
		else
		{
			userEmpty.IsVisible = false;
		}

		// input sanitization
		string _username = username.Text.Trim();
        // get user
        user = await DatabaseService.GetUser(_username);
		// check if user is null
        if (user == null)
		{
			userNotFound.IsVisible = true;
			return;
		}
		else
		{
			userNotFound.IsVisible = false;
		}

		// display password
		passwordLabel.IsVisible = true;
		passwordText.IsVisible = true;
		passwordText.Text = user.Password;
	}
}