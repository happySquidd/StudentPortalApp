using StudentPortalApp.Models;
using StudentPortalApp.Services;

namespace StudentPortalApp.Views;

public partial class EditTerm : ContentPage
{
    private int termId;
	public EditTerm(Term term)
	{
		InitializeComponent();

        // populate fields with existing term data
        TermName.Text = term.Name;
        TermStart.Date = term.StartTime;
        TermEnd.Date = term.EndTime;

        // get term id
        termId = term.Id;
	}

	private async void ConfirmEdit(object sender, EventArgs e)
	{
        // get data from fields
        string termName = TermName.Text;
        DateTime startDate = TermStart.Date;
        DateTime endDate = TermEnd.Date;

        if (string.IsNullOrEmpty(termName))
        {
            // if no name given, default to "Year Term"
            termName = startDate.Year.ToString() + " Term";
        }
        if (endDate <= startDate)
        {
            await DisplayAlert(
                title: "Error", 
                message: "The end date cannot be earlier or the same as the start date", 
                cancel: "OK");
            return;
        }

        await DatabaseService.UpdateTerm(
            id: termId,
            name: termName,
            startTime: startDate.ToString(),
            endTime: endDate.ToString()
            );

        await Navigation.PopAsync();
	}

    private async void DeleteTerm(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert(
            title: "Confirm Delete",
            message: "Are you sure you want to delete this term?",
            accept: "Yes",
            cancel: "No");
        if (confirm)
        {
            await DatabaseService.RemoveTerm(termId);
            await Navigation.PopAsync();
        }
    }

    private async void Close(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}
}