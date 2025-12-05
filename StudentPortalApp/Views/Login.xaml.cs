using StudentPortalApp.Models;
using StudentPortalApp.Services;

namespace StudentPortalApp.Views;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
    }

	protected override async void OnAppearing()
	{
		if (Settings.FirstRun)
		{
			await DatabaseService.LoadSampleData();
			Settings.FirstRun = false;
		}

		base.OnAppearing();
	}

	private async void LoginClicked(object sender, EventArgs e)
	{
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


        // verify username and password
        User loginUser = await VerifyUsername(username.Text);
        if (loginUser != null)
        {
            userNotFound.IsVisible = false;
        }
        else
        {
            // user not found
            userNotFound.IsVisible = true;
            return;
        }
        
        if (await VerifyPassword(loginUser, password.Text))
        {
            passwordIncorrect.IsVisible = false;
        }
        else
        {
            passwordIncorrect.IsVisible = true;
            return;
        }

        // successfull login

        // create a new root window which is the terms view page
        var terms = new TermsPage(loginUser.Id, loginUser.UserName);
        var TermsPage = new NavigationPage(terms);
        Application.Current.Windows[0].Page = TermsPage;

	}

	public static async Task<User> VerifyUsername(string username)
	{
        // input sanitization
        string _username = username.Trim();
        // fetch user
        User user = await DatabaseService.GetUser(_username);
        if (user == null)
        {
            // user not found
            return null;
        }
        
        return user;
    }

    public static async Task<bool> VerifyPassword(User user, string password)
    {
        string _password = password.Trim();
        if (!await DatabaseService.VerifyPassword(user, _password))
        {
            // passwords don't match
            return false;
        }
        return true;
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
	// last 2 functions are for debug purposes, add this code back into the xaml to use them
	//<ContentPage.ToolbarItems>
    //    <ToolbarItem Text = "Reset sample data"
    //             Clicked="ResetSampleData"/>
    //    <ToolbarItem Text = "Clear all data"
    //             Clicked="ClearSampleData"/>
    //</ContentPage.ToolbarItems>
}