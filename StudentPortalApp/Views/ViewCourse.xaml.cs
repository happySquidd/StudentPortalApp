using StudentPortalApp.Models;
using StudentPortalApp.Services;

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

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await LoadAssessments();
    }

    private async Task LoadAssessments()
    {
        var objective = await DatabaseService.GetAssessmentByType(course.Id, "Objective Assessment");
        if (objective != null)
        {
            OAssessmentBorder.IsVisible = true;
            OAName.Text = objective.Name;
            OAType.Text = objective.Type;
            OAStartDate.Text = objective.StartTime.ToString("MM/dd/yyyy");
            OAEndDate.Text = objective.EndTime.ToString("MM/dd/yyyy");
            OAStatus.Text = objective.Status;
            OAStartReminder.Text = objective.StartNotification ? "Yes" : "No";
            OAEndReminder.Text = objective.EndNotification ? "Yes" : "No";
        }
        else
        {
            OAssessmentBorder.IsVisible = false;
        }

        var performance = await DatabaseService.GetAssessmentByType(course.Id, "Performance Assessment");
        if (performance != null)
        {
            PAssessmentBorder.IsVisible = true;
            PAName.Text = performance.Name;
            PAType.Text = performance.Type;
            PAStartDate.Text = performance.StartTime.ToString("MM/dd/yyyy");
            PAEndDate.Text = performance.EndTime.ToString("MM/dd/yyyy");
            PAStatus.Text = performance.Status;
            PAStartReminder.Text = performance.StartNotification ? "Yes" : "No";
            PAEndReminder.Text = performance.EndNotification ? "Yes" : "No";
        }
        else
        {
            PAssessmentBorder.IsVisible = false;
        }

		if (performance == null && objective == null)
		{
			NoAssessmentsLabel.IsVisible = true;
			NoAssessmentsLabel.Text = "No assessments are associated with this course";
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