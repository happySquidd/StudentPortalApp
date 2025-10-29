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

        // Set notification text based on course notification settings
        if (!course.StartNotification && !course.EndNotification)
		{
			Notifications.Text = "All notifcations are disabled for this course";
		}
		else if (course.StartNotification && !course.EndNotification)
		{
			Notifications.Text = "You enabled a notification for the start of this course";
		}
		else if (course.EndNotification && !course.StartNotification)
		{
			Notifications.Text = "You enabled a notification for the end of this course";
		}
		else
		{
			Notifications.Text = "You enabled notifications for start and end of this course";
		}
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