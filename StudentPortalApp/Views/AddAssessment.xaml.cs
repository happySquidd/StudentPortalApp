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

        string type = NewType.SelectedItem.ToString()!;
        string name = NewName.Text;
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