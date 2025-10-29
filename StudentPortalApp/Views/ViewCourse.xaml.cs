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

		LoadData();
    }


	private async void LoadData()
	{
		// get and display assessments for this course
		var oassessment = await DatabaseService.GetAssessmentByType(course.Id, "Objective Assessment");
		if (oassessment != null)
		{
			OAName.Text = oassessment.Name;
			OAStatus.Text = oassessment.Status;
			OAStartDate.Text = oassessment.StartTime.ToString("MM/dd/yyyy");
			OAEndDate.Text = oassessment.EndTime.ToString("MM/dd/yyyy");
            OAssessmentBorder.IsVisible = true;
        }
		var passessment = await DatabaseService.GetAssessmentByType(course.Id, "Performance Assessment");
		if (passessment != null)
		{
			PAName.Text = passessment.Name;
			PAStatus.Text = passessment.Status;
			PAStartDate.Text = passessment.StartTime.ToString("MM/dd/yyyy");
			PAEndDate.Text = passessment.EndTime.ToString("MM/dd/yyyy");
            PAssessmentBorder.IsVisible = true;
        }
		if (oassessment == null && passessment == null)
		{
			NoAssessmentsLabel.Text = "There are no assessments for this course.";
            NoAssessmentsLabel.IsVisible = true;
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