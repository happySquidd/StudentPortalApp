using Plugin.LocalNotification;
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
		CourseNotes.Text = course.Notes;
		StartNotificationSwitch.IsToggled = course.StartNotification;
		EndNotificationSwitch.IsToggled = course.EndNotification;

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
		string notes = CourseNotes.Text;
		bool startNotif = StartNotificationSwitch.IsToggled;
		bool endNotif = EndNotificationSwitch.IsToggled;

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
			notes: notes,
			startNotification: startNotif,
			endNotification: endNotif,
			dueDate: course.DueDate);

		// notifications
		if (startNotif)
		{
			var startRequest = new NotificationRequest
			{
				Title = "Course Starting",
				Description = $"The course '{courseName}' starts today.",
				Schedule = new NotificationRequestSchedule
				{
					NotifyTime = start
				}
			};
			await LocalNotificationCenter.Current.Show(startRequest);
        }
		if (endNotif)
		{
			var endRequest = new NotificationRequest
			{
				Title = "Course Ending",
				Description = $"The course '{courseName}' ends today.",
				Schedule = new NotificationRequestSchedule
				{
					NotifyTime = end
                }
			};
			await LocalNotificationCenter.Current.Show(endRequest);
        }

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

	private async void ShareCourse(object sender, EventArgs e)
	{
		if (string.IsNullOrWhiteSpace(CourseNotes.Text))
		{
			await DisplayAlert(
				title: "Warning",
				message: "Can't share empty notes",
				cancel: "OK");
			return;
		}

		await Share.Default.RequestAsync(new ShareTextRequest
		{
			Text = CourseNotes.Text,
			Title = "Share Course Notes"
		});
	}
}