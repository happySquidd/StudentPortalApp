using StudentPortalApp.Services;
using System.Threading.Tasks;

namespace StudentPortalApp.Views;

public partial class AddTerm : ContentPage
{
	public AddTerm()
	{
		InitializeComponent();
	}


    private async void ConfirmAddTerm(object sender, EventArgs e)
    {
        // input sanitization
        string termName = NewTermName.Text.Trim();
        DateTime startDate = NewTermStartDate.Date;
        DateTime endDate = NewTermEndDate.Date;

        if (string.IsNullOrWhiteSpace(termName))
        {
            // if no name given, default to "Year Term"
            termName = startDate.Year.ToString() + " Term";
        }
        if (endDate <= startDate)
        {
            await DisplayAlert(title: "Error", message:"The end date cannot be earlier or the same as the start date", cancel:"OK");
            return;
        }
        await DatabaseService.AddTerm(TermsPage.UserId, termName, startDate, endDate);
        await Navigation.PopAsync();
    }

    private async void ClosePopup(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}