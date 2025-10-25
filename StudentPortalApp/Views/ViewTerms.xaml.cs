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

    private async void AddTerm(object sender, EventArgs e)
    {
        await DatabaseService.AddTerm(name: "New Term", startTime: DateTime.Now, endTime: DateTime.Now.AddMonths(6));
        await LoadData();
    }

    private async void ViewTerm(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TermWindow());
    }

    private async void EditName(object sender, EventArgs e)
    {
        //var term = sender as Term;
        if (true)
        {
            string result = await DisplayPromptAsync(
                title: "Edit Term Name", 
                message: "Enter new term name:", 
                accept: "Save",
                cancel: "Cancel"
                //initialValue: term.Name
            );

            if (!string.IsNullOrWhiteSpace(result))
            {
                //term.Name = result;
                //await DatabaseService.UpdateTerm(id: term.Id, name: term.Name, startTime: term.StartTime.ToString(), endTime: term.EndTime.ToString());
                //await LoadData();
            }
        }
    }
    private async void DeleteTerm(object sender, EventArgs e)
    {
        //
    }

    private async void DeleteName(object sender, EventArgs e)
    {

    }

}