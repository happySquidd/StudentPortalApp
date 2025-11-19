using StudentPortalApp.Models;
using StudentPortalApp.Services;

namespace StudentPortalApp.Views;

public partial class ChangePassword : ContentPage
{
	private User user;
	public ChangePassword(User user)
	{
		InitializeComponent();

		this.user = user;
	}

	private async void ConfirmClicked(object sender, EventArgs e)
	{
		// input validation
		if (string.IsNullOrWhiteSpace(newPassword.Text))
		{
			passwordLabel.IsVisible = true;
			return;
		}
		else
		{
			passwordLabel.IsVisible = false;
			// input sanitization
			string _newPassword = newPassword.Text.Trim();
			// change the password in the user
			user.Password = _newPassword;
		}

		// update the user in the database
		await DatabaseService.UpdateUser(user);
		await DisplayAlert(
			title: "Success",
			message: "Successfully updated password",
			cancel: "OK");
		await Navigation.PopAsync();
	}

	private async void CancelClicked(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}
}