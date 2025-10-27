namespace StudentPortalApp.Views;
using StudentPortalApp.Models;
using StudentPortalApp.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

public partial class TermWindow : ContentPage
{
	public ObservableCollection<Course> Courses { get; set; }
	private int termId;
	public TermWindow(Term term)
	{
		InitializeComponent();

		Courses = new ObservableCollection<Course>();
		this.BindingContext = this;
		termId = term.Id;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadData(termId);
    }

    private async Task LoadData(int termId)
	{
		Courses.Clear();
		var coursesList = await DatabaseService.GetCoursesByTerm(termId);
		foreach (var course in coursesList)
		{
			Courses.Add(course);
		}

		if (coursesList.Count == 0)
		{
			NoCoursesLabel.IsVisible = true;
			
		}
	}

	private async void AddCourse(object sender, EventArgs e)
	{
		await DatabaseService.AddCourse(
			termId: termId,
			name: "New Course",
			startTime: DateTime.Now.ToString(),
			endTime: DateTime.Now.ToString(),
			status: "Not started",
			instructorName: "Unknown",
			instructorPhone: "",
			instructorEmail: "",
			notes: "",
			startNotification: false,
			endNotification: false,
			dueDate: ""
			);
		await LoadData(termId);
	}

	private async void DeleteCourse(object sender, EventArgs e)
	{
		var button = (Button)sender;
		var course = button.BindingContext as Course;
        if (course != null) 
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
				await LoadData(termId);
			}
        }
    }
}