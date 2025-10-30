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
		else
		{
			NoCoursesLabel.IsVisible = false;
		}
	}

	private async void AddCourse(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new AddCourse(termId));
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

	private async void EditCourse(object sender, EventArgs e)
	{
		var button = (Button)sender;
		var course = button.BindingContext as Course;
		if (course != null)
		{
			await Navigation.PushAsync(new EditCourse(course));
		}
	}

	private async void ViewCourse(object sender, EventArgs e)
	{
		var button = (Button)sender;
		var course = button.BindingContext as Course;
		if (course != null)
		{
			await Navigation.PushAsync(new ViewCourse(course));
		}
    }
}