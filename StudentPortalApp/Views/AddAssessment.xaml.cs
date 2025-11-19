using Plugin.LocalNotification;
using StudentPortalApp.Services;

namespace StudentPortalApp.Views;

public partial class AddAssessment : ContentPage
{
    private readonly int courseId;
	public AddAssessment(int courseId)
	{
		InitializeComponent();

        this.courseId = courseId;
    }

	private async void ConfirmAddAssessment(object sender, EventArgs e)
	{
        // input validation
        if (NewType.SelectedItem == null || AssessmentStatus.SelectedItem == null)
        {
            await DisplayAlert(
                title: "Error",
                message: "Please select an assessment type and status",
                cancel: "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(NewName.Text))
        {
            await DisplayAlert(
                title: "Error",
                message: "Assessment name cannot be empty",
                cancel: "OK");
            return;
        }
        if (NewEndDate.Date <= NewStartDate.Date)
        {
            await DisplayAlert(
                title: "Error",
                message: "The end date cannot be earlier or the same as the start date",
                cancel: "OK");
            return;
        }

        // input sanitization
        string newName = NewName.Text.Trim();

        // see if assessment of this type already exists for this course
        var existingAssessment = await DatabaseService.GetAssessmentByType(courseId, NewType.SelectedItem.ToString()!);
        if (existingAssessment != null)
        {
            await DisplayAlert(
                title: "Error",
                message: "An assessment of this type already exists for this course",
                cancel: "OK");
            return;
        }

        // set up notifications
        if (StartNotificationSwitch.IsToggled)
        {
            bool soon = false;
            if (NewStartDate.Date == DateTime.Today)
            {
                soon = true;
            }
                var request = new NotificationRequest
                {
                    NotificationId = 0,
                    Title = "Assessment Starting",
                    Description = $"The assessment '{newName}' starts today",
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = soon ? DateTime.Now.AddSeconds(2) : NewStartDate.Date,
                    }
                };
            await LocalNotificationCenter.Current.Show(request);
        }
        if (EndNotificationSwitch.IsToggled)
        {
            bool soon = false;
            if (NewEndDate.Date == DateTime.Today)
            {
                soon = true;
            }
            var request = new NotificationRequest
            {
                NotificationId = 1,
                Title = "Assessment Ending",
                Description = $"The assessment '{newName}' ends today",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = soon ? DateTime.Now.AddSeconds(2) : NewEndDate.Date,
                }
            };
            await LocalNotificationCenter.Current.Show(request);
        }

        // update database
        string type = NewType.SelectedItem.ToString()!;
        string name = newName;
        string start = NewStartDate.Date.ToString();
        string end = NewEndDate.Date.ToString();
        string status = AssessmentStatus.SelectedItem.ToString()!;
        bool startNotif = StartNotificationSwitch.IsToggled;
        bool endNotif = EndNotificationSwitch.IsToggled;

        await DatabaseService.AddAssessment(
                courseId: courseId,
                type: type,
                name: name,
                start: start,
                end: end,
                status: status,
                startNotification: startNotif,
                endNotification: endNotif);

        await Navigation.PopAsync();
    }

	private async void ClosePopup(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}
}