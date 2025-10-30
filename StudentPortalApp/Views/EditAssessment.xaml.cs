using StudentPortalApp.Models;
using StudentPortalApp.Services;

namespace StudentPortalApp.Views;

public partial class EditAssessment : ContentPage
{
	private readonly Assessment assessment;
    public EditAssessment(Assessment assessment)
	{
		InitializeComponent();

		this.assessment = assessment;

		// fill in fields
		Type.SelectedItem = assessment.Type;
		Name.Text = assessment.Name;
		StartDate.Date = assessment.StartTime;
		EndDate.Date = assessment.EndTime;
        AssessmentStatus.SelectedItem = assessment.Status;
		StartNotificationSwitch.IsToggled = assessment.StartNotification;
		EndNotificationSwitch.IsToggled = assessment.EndNotification;
    }

	private async void ConfirmClicked(object sender, EventArgs e)
	{
		// input validation
		if (Type.SelectedItem == null || AssessmentStatus == null)
		{
			await DisplayAlert(
				title: "Error",
				message: "Please select an assessment type and status",
				cancel: "OK");
			return;
		}
		if (string.IsNullOrWhiteSpace(Name.Text))
		{
			await DisplayAlert(
				title: "Error",
				message: "Assessment name cannot be empty",
				cancel: "OK");
			return;
		}
		if (EndDate.Date <= StartDate.Date)
		{
			await DisplayAlert(
				title: "Error",
				message: "The end date cannot be earlier or the same as the start date",
				cancel: "OK");
			return;
        }

        // see if assessment of this type already exists for this course
		var existingAssessment = await DatabaseService.GetAssessmentByType(assessment.CourseId, Type.SelectedItem.ToString()!);
		if (existingAssessment != null && existingAssessment.Id != assessment.Id)
		{
			await DisplayAlert(
				title: "Error",
				message: "An assessment of this type already exists for this course",
				cancel: "OK");
			return;
        }

        string type = Type.SelectedItem.ToString()!;
		string name = Name.Text;
		string start = StartDate.Date.ToString();
		string end = EndDate.Date.ToString();
		string status = AssessmentStatus.SelectedItem.ToString()!;
		bool startNotif = StartNotificationSwitch.IsToggled;
		bool endNotif = EndNotificationSwitch.IsToggled;

		await DatabaseService.UpdateAssessment(
			id: assessment.Id,
			courseId: assessment.CourseId,
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

	private async void DeleteClicked(object sender, EventArgs e)
	{
		bool confirm = await DisplayAlert(
			title: "Confirm Delete",
			message: "Are you sure you want to delete this assessment?",
			cancel: "Cancel",
			accept: "Delete");
		if (confirm)
		{
			await DatabaseService.RemoveAssessment(assessment.Id);
			await Navigation.PopAsync();
        }
    }
}