using StudentPortalApp.Models;
using StudentPortalApp.Services;

namespace StudentPortalApp.Views;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
    }

	//protected override async void OnAppearing()
	//{
	//if (Settings.FirstRun)
	//{
	//    await DatabaseService.LoadSampleData();
	//    Settings.FirstRun = false;
	//}

	//       base.OnAppearing();
	//}

	private async void LoginClicked(object sender, EventArgs e)
	{
		User user = new User();
        if (!string.IsNullOrWhiteSpace(username.Text) && !string.IsNullOrWhiteSpace(password.Text))
        {
            user = await DatabaseService.GetUser(username.Text);
        }
		else
		{
			await DisplayAlert(
				title: "Error",
				message: "Please enter your username and password",
				cancel: "OK");
			return;
		}
		// user not found
		if (user == null)
		{
			await DisplayAlert(
				title: "Error",
				message: "User not found",
				cancel: "OK");
			return;
		}
		// passwords don't match
		if (!await DatabaseService.VerifyPassword(user, password.Text))
		{
			await DisplayAlert(
				title: "Error",
				message: "Password is incorrect",
				cancel: "OK");
			return;
		}
		// successfull login

        // create a new root window which is the terms view page
        var terms = new TermsPage(user.Id);
		var TermsPage = new NavigationPage(terms);
		Application.Current.Windows[0].Page = TermsPage;
	}

	private void RegisterClicked(object sender, EventArgs e)
	{
		var register = new Register();
		var RegPage = new NavigationPage(register);
		Application.Current.Windows[0].Page = RegPage;
	}

    private async void ResetSampleData(object sender, EventArgs e)
    {
        await DatabaseService.ClearSampleData();
        await DatabaseService.LoadSampleData();
    }

    private async void ClearSampleData(object sender, EventArgs e)
    {
        await DatabaseService.ClearSampleData();
    }
}