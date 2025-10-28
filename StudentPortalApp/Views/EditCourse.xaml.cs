using StudentPortalApp.Models;
using StudentPortalApp.Services;

namespace StudentPortalApp.Views;

public partial class EditCourse : ContentPage
{
	private readonly Course course;
    public EditCourse(Course course)
	{
		InitializeComponent();

		CourseName.Text = course.Name;
		CourseStart.Date = course.StartTime;
		CourseEnd.Date = course.EndTime;
		CourseStatus.SelectedItem = course.Status;
		InstName.Text = course.InstructorName;
		InstPhone.Text = course.InstructorPhone;
		InstEmail.Text = course.InstructorEmail;

		this.course = course;
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

		await DatabaseService.UpdateCourse(
			id: course.Id,
			termId: course.TermId,
			name: courseName,
			startTime: start.ToString(),
			endTime: end.ToString(),
			status: status,
			instructorName: instructorName,
			instructorPhone: instructorPhone,
			instructorEmail: instructorEmail,
			notes: course.Notes,
			startNotification: course.StartNotification,
			endNotification: course.EndNotification,
			dueDate: course.DueDate);
		await Navigation.PopAsync();
    }

    private async void DeleteCourse(object sender, EventArgs e)
    {
        int courseId = course.Id;
        var answer = await DisplayAlert(
            title: "Confirm Delete",
            message: "Are you sure you want to delete this course?",
            cancel: "Cancel",
            accept: "Yes");
        if (answer)
        {
            await DatabaseService.RemoveCourse(courseId);
			await Navigation.PopAsync();
        }
    }

    private async void CancelClick(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}
}