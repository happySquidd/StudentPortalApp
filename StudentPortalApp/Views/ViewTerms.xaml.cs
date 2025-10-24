using StudentPortalApp.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using StudentPortalApp.Services;

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
    }

    private async void ViewTerm(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TermWindow());
    }

}