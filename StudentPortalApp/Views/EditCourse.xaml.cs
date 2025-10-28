using StudentPortalApp.Models;

namespace StudentPortalApp.Views;

public partial class EditCourse : ContentPage
{
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
	}
}