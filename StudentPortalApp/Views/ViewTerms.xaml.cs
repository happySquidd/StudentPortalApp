using StudentPortalApp.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using StudentPortalApp.Services;
using Plugin.LocalNotification;

namespace StudentPortalApp.Views;

public partial class TermsPage : ContentPage
{
    public ObservableCollection<Term> Terms { get; set; }
    public static int UserId { get; set; }
    private static string? Username { get; set; }
    public TermsPage(int userId, string username)
	{
		InitializeComponent();

        UserId = userId;
        Username = username;
        Terms = new ObservableCollection<Term>();
        this.BindingContext = this;

    }

    protected override async void OnAppearing()
    {
        if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
        {
            await LocalNotificationCenter.Current.RequestNotificationPermission();
        }

        base.OnAppearing();
        await LoadData();
    }

    private async Task LoadData()
    {
        Terms.Clear();
        var terms = await DatabaseService.GetTerms(UserId);
        if (terms == null || terms.Count == 0)
        {
            return;
        }
        foreach (var term in terms)
        {
            Terms.Add(term);
        }
        Settings.FirstRun = false;
    }

    private async void AddTerm(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddTerm());

    }

    private async void ViewTerm(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var term = button.BindingContext as Term;
        if (term != null)
        {
            await Navigation.PushAsync(new TermWindow(term));
        }
    }

    private async void EditTerm(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var term = button.BindingContext as Term;
        if (term != null)
        {
            await Navigation.PushAsync(new EditTerm(term));
        }
    }

    private async void SettingsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EditUser(Username));
    }
}