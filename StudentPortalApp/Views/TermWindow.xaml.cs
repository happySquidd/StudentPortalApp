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

	private void AddCourse(object sender, EventArgs e)
	{
		Courses.Add(new Course { Name = "New Course" , StartTime = DateTime.Now, EndTime = DateTime.Now, Status = "Not started"});
	}

	private async void DeleteCourse(object sender, EventArgs e)
	{

	}
}