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

    }

    private async void ClosePopup(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}