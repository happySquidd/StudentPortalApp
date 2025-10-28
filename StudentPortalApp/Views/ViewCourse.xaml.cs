using StudentPortalApp.Models;

namespace StudentPortalApp.Views;

public partial class ViewCourse : ContentPage
{
	public ViewCourse(Course course)
	{
		InitializeComponent();

		this.BindingContext = course;
	}
}