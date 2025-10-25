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
        await Navigation.PushAsync(new AddTerm());

    }

    private async void ViewTerm(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TermWindow());
    }

    private async void EditName(object sender, EventArgs e)
    {
        // get the term to update its name
        var button = (Button)sender;
        var term = button.BindingContext as Term;
        if (term != null)
        {
            string result = await DisplayPromptAsync(
                title: "Edit Term Name", 
                message: "Enter new term name:", 
                accept: "Save",
                cancel: "Cancel",
                initialValue: term.Name
            );

            if (!string.IsNullOrWhiteSpace(result))
            {
                term.Name = result;
                await DatabaseService.UpdateTerm(id: term.Id, name: term.Name, startTime: term.StartTime.ToString(), endTime: term.EndTime.ToString());
                await LoadData();
            }
        }
    }
    private async void DeleteTerm(object sender, EventArgs e)
    {
        // get the term to pull id
        var button = (Button)sender;
        var term = button.BindingContext as Term;
        if (term != null)
        {
            bool confirm = await DisplayAlert(
                title: "Confirm Delete",
                message: "Are you sure you want to delete this term?",
                accept: "Yes",
                cancel: "No");
            if (confirm)
            {
                int Id = term.Id;
                await DatabaseService.RemoveTerm(Id);
                await LoadData();
            }
        }
    }

    private async void DeleteName(object sender, EventArgs e)
    {
        // get the term to get its year
        var button = (Button)sender;
        var term = button.BindingContext as Term;

        if (term != null)
        {
            bool confirm = await DisplayAlert(
                title: "Confirm",
                message: "Are you sure you want to reset this term name?",
                accept: "Yes",
                cancel: "No");
            if (confirm)
            {
                string year = term.StartTime.Year.ToString();
                await DatabaseService.UpdateTerm(id: term.Id, name: (year + " Term"), startTime: term.StartTime.ToString(), endTime: term.EndTime.ToString());
                await LoadData();
            }
        }
    }

}