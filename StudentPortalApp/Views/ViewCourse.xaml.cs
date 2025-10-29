using StudentPortalApp.Models;

namespace StudentPortalApp.Views;

public partial class ViewCourse : ContentPage
{
	private readonly Course course;
	public ViewCourse(Course course)
	{
		InitializeComponent();

		this.BindingContext = course;
		this.course = course;
	}

	private async void ShareCourse(object sender, EventArgs e)
	{
		if (string.IsNullOrWhiteSpace(course.Notes))
		{
			await DisplayAlert(
				title: "Error",
				message: "There are no notes to share for this course.",
				cancel: "OK");
			return;
        }
		await Share.Default.RequestAsync(new ShareTextRequest
		{
			Text = course.Notes,
			Title = "Share Course Notes"
		});
	}
}