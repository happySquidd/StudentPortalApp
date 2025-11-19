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
		if (string.IsNullOrWhiteSpace(password.Text))
		{
			passwordEmpty.IsVisible = true;
			return;
		}
		else
		{
			passwordEmpty.IsVisible = false;
		}

		// input sanitization
		string _username = username.Text.Trim();
		string _password = password.Text.Trim();
        // fetch user
        user = await DatabaseService.GetUser(_username);
        // user not found
        if (user == null)
		{
			userNotFound.IsVisible = true;
			return;
		}
        else
        {
			userNotFound.IsVisible = false;
        }
		// passwords don't match
		if (!await DatabaseService.VerifyPassword(user, _password))
		{
			passwordIncorrect.IsVisible = true;
			return;
		}
        // successfull login
        
        passwordIncorrect.IsVisible = false;
        // create a new root window which is the terms view page
        var terms = new TermsPage(user.Id, user.UserName);
		var TermsPage = new NavigationPage(terms);
		Application.Current.Windows[0].Page = TermsPage;
	}

	private void RegisterClicked(object sender, EventArgs e)
	{
		var register = new Register();
		var RegPage = new NavigationPage(register);
		Application.Current.Windows[0].Page = RegPage;
	}

	private async void ForgotClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new ForgotPassword());
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