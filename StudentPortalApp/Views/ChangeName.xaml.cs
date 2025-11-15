using StudentPortalApp.Models;
using StudentPortalApp.Services;

namespace StudentPortalApp.Views;

public partial class ChangeName : ContentPage
{
	private User user;
	public ChangeName(User user)
	{
		InitializeComponent();

		username.Text = user.UserName;
		this.user = user;
	}

	private async void ConfirmClicked(object sender, EventArgs e)
	{
		if (string.IsNullOrWhiteSpace(username.Text))
		{
			usernameLabel.IsVisible = true;
			return;
		}
		else
		{
			usernameLabel.IsVisible = false;
			// change the name
			user.UserName = username.Text;
		}

		// if username is taken this will return false, updates if true
		bool success = await DatabaseService.UpdateUser(user);
        if (!success)
		{
			usernameTaken.IsVisible = true;
			return;
		}

		usernameTaken.IsVisible = false;
		await DisplayAlert(
			title: "Success",
			message: "Successfully updated username",
			cancel: "OK");
		await Navigation.PopAsync();
	}

	private async void CancelClicked(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}
}