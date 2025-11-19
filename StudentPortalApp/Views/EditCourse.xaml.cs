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
		DueDate.Date = course.DueDate;

		this.course = course;

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

		// get data from fields & input sanitization
		string courseName = CourseName.Text.Trim();
		DateTime start = CourseStart.Date;
		DateTime end = CourseEnd.Date;
		string status = CourseStatus.SelectedItem.ToString()!;
		string instructorName = InstName.Text.Trim();
		string instructorPhone = InstPhone.Text.Trim();
		string instructorEmail = InstEmail.Text.Trim();
		string notes = CourseNotes.Text.Trim();
		bool startNotif = StartNotificationSwitch.IsToggled;
		bool endNotif = EndNotificationSwitch.IsToggled;
		DateTime dueDate = DueDate.Date;

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
			dueDate: dueDate.ToString());

		// notifications
		if (startNotif)
		{
			bool soon = false;
			if (start.Date == DateTime.Today) { soon = true; }
            var startRequest = new NotificationRequest
			{
				NotificationId = 2,
				Title = "Course Starting",
				Description = $"The course '{courseName}' starts today.",
				Schedule = new NotificationRequestSchedule
				{
					NotifyTime = soon ? DateTime.Now.AddSeconds(2) : start
                }
			};
			await LocalNotificationCenter.Current.Show(startRequest);
		}
		if (endNotif)
		{
			bool soon = false;
			if (end.Date == DateTime.Today) { soon = true; }
            var endRequest = new NotificationRequest
			{
				NotificationId = 3,
				Title = "Course Ending",
				Description = $"The course '{courseName}' ends today.",
				Schedule = new NotificationRequestSchedule
				{
					NotifyTime = soon ? DateTime.Now.AddSeconds(2) : end
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

	private async void AddAssessment(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new AddAssessment(course.Id));

    }

	private async void AddPerformance(object sender, EventArgs e)
	{
		var performance = await DatabaseService.GetAssessmentByType(course.Id, "Performance Assessment");
		if (performance != null)
		{
			await DisplayAlert(
				title: "Warning",
				message: "A Performance Assessment already exists for this course",
				cancel: "OK");
			return;
		}
		else
		{
			await DatabaseService.AddAssessment(
				courseId: course.Id,
				type: "Performance Assessment",
				name: "New performance assessment",
				start: DateTime.Now.ToString(),
				end: DateTime.Now.AddDays(7).ToString(),
				status: "Planned",
				startNotification: false,
				endNotification: false);

			await DisplayAlert(
				title: "Success",
				message: "Performance Assessment added",
				cancel: "OK");
        }
	}

	private void Refresh(object sender, EventArgs e)
	{
		LoadAssessments();
	}

	private async void EditAssessmentOA(object sender, EventArgs e)
	{
		var objective = await DatabaseService.GetAssessmentByType(course.Id, "Objective Assessment");
        await Navigation.PushAsync(new EditAssessment(objective));
		LoadAssessments();
	}

	private async void EditAssessmentPA(object sender, EventArgs e)
	{
		var performance = await DatabaseService.GetAssessmentByType(course.Id, "Performance Assessment");
		await Navigation.PushAsync(new EditAssessment(performance));
		LoadAssessments();
    }
}