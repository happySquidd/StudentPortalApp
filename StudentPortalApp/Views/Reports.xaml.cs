using StudentPortalApp.Models;
using StudentPortalApp.Services;
using System.Collections.ObjectModel;

namespace StudentPortalApp.Views;

public partial class Reports : ContentPage
{
	public ObservableCollection<Course> Courses { get; set; }
	private static string _username;
	public Reports(string username)
	{
		InitializeComponent();
		Courses = new ObservableCollection<Course>();
		_username = username;
		this.BindingContext = this;
	}

	private async void SearchClicked(object sender, EventArgs e)
	{
		if (string.IsNullOrWhiteSpace(searchBar.Text))
		{
			emptySearchBar.IsVisible = true;
			return;
		}
		emptySearchBar.IsVisible = false;
		Courses.Clear();
		// input sanitization 
		string _searchParam = searchBar.Text.Trim();

		// find all terms tied to the user to then find all user courses
		var user = await DatabaseService.GetUser(_username);
        var terms = await DatabaseService.GetTerms(user.Id);
		foreach(var term in terms)
		{
			var courses = await DatabaseService.GetCoursesByTerm(term.Id);
			foreach(var course in courses)
			{
				// see if the keyword is in the name, add if yes
				if (course.Name.ToLower().Contains(_searchParam.ToLower()))
				{
					Courses.Add(course);
				}
			}
		}

		if(Courses.Count > 0)
		{
			NoCoursesLabel.IsVisible = false;
			CourseView.IsVisible = true;
		}
		else
		{
			NoCoursesLabel.IsVisible = true;
			CourseView.IsVisible = false;
		}

    }
}