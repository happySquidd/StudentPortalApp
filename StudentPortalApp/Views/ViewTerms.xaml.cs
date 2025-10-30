using StudentPortalApp.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using StudentPortalApp.Services;
using Plugin.LocalNotification;

namespace StudentPortalApp.Views;

public partial class TermsPage : ContentPage
{
    public ObservableCollection<Term> Terms { get; set; }
    public TermsPage()
	{
		InitializeComponent();

        Terms = new ObservableCollection<Term>();
        this.BindingContext = this;

    }

    protected override async void OnAppearing()
    {
        if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
        {
            await LocalNotificationCenter.Current.RequestNotificationPermission();
        }

        if (Settings.FirstRun)
        {
            await DatabaseService.LoadSampleData();
            Settings.FirstRun = false;
        }

        base.OnAppearing();
        await LoadData();
    }

    private async Task LoadData()
    {
        Terms.Clear();
        var terms = await DatabaseService.GetTerms();
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

    private async void ResetSampleData(object sender, EventArgs e)
    {
        await DatabaseService.ClearSampleData();
        await LoadData();
        await DatabaseService.LoadSampleData();
        await LoadData();
    }

    private async void ClearSampleData(object sender, EventArgs e)
    {
        await DatabaseService.ClearSampleData();
        await LoadData();
    }
}