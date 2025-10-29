using StudentPortalApp.Models;
using StudentPortalApp.Services;

namespace StudentPortalApp.Views;

public partial class AddCourse : ContentPage
{
	private readonly int termId;
    public AddCourse(int termId)
	{
		InitializeComponent();

		this.termId = termId;
	}

	private async void SaveClick(object sender, EventArgs e)
	{
        // input validation
        if (string.IsNullOrWhiteSpace(CourseName.Text))
        {
            await DisplayAlert(
                title: "Error",
                message: "Course name cannot be empty",
                cancel: "OK");
            return;
        }
        if (CourseEnd.Date <= CourseStart.Date)
        {
            await DisplayAlert(
                title: "Error",
                message: "The end date cannot be earlier or the same as the start date",
                cancel: "OK");
            return;
        }
        if (CourseStatus.SelectedItem == null)
        {
            await DisplayAlert(
                title: "Error",
                message: "Please select a course status",
                cancel: "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(InstName.Text) || string.IsNullOrWhiteSpace(InstPhone.Text))
        {
            await DisplayAlert(
                title: "Error",
                message: "Instructor name and phone cannot be empty",
                cancel: "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(InstEmail.Text) || !InstEmail.Text.Contains("@"))
        {
            await DisplayAlert(
                title: "Error",
                message: "Please enter a valid instructor email",
                cancel: "OK");
            return;
        }

        // get data from fields
        string courseName = CourseName.Text;
        DateTime start = CourseStart.Date;
        DateTime end = CourseEnd.Date;
        string status = CourseStatus.SelectedItem.ToString();
        string instructorName = InstName.Text;
        string instructorPhone = InstPhone.Text;
        string instructorEmail = InstEmail.Text;

        await DatabaseService.AddCourse(
            termId: termId,
            name: courseName,
            startTime: start.ToString(),
            endTime: end.ToString(),
            status: status,
            instructorName: instructorName,
            instructorPhone: instructorPhone,
            instructorEmail: instructorEmail,
            notes: CourseNotes.Text,
            startNotification: false,
            endNotification: false,
            dueDate: "");
        await Navigation.PopAsync();
    }

    private async void CancelClick(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}
}