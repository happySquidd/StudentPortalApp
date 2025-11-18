using StudentPortalApp.Models;
using StudentPortalApp.Services;
using System.Collections.ObjectModel;

namespace StudentPortalApp.Views;

public partial class Reports : ContentPage
{
	private ObservableCollection<Course> _courses;
	private static string _username;
	public Reports(string username)
	{
		InitializeComponent();
		_courses = new ObservableCollection<Course>();
		_username = username;
	}

	private async void SearchClicked(object sender, EventArgs e)
	{
		_courses.Clear();
		// find all terms tied to the user to then find all user courses
		var user = await DatabaseService.GetUser(_username);
        var terms = await DatabaseService.GetTerms(user.Id);
		foreach(var term in terms)
		{
			var courses = await DatabaseService.GetCoursesByTerm(term.Id);
			foreach(var course in courses)
			{
				_courses.Add(course);
			}
		}

    }
}