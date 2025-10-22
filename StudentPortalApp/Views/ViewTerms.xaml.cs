using System.Threading.Tasks;

namespace StudentPortalApp.Views;

public partial class TermsPage : ContentPage
{
	public TermsPage()
	{
		InitializeComponent();
	}

    private async void AddTerm_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddTerm());
    }

    private void ClearDb_Clicked(object sender, EventArgs e)
    {

    }

    private void LoadDb_Clicked(object sender, EventArgs e)
    {

    }
}