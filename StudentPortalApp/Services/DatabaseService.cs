using SQLite;
using StudentPortalApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPortalApp.Services
{
    public static class DatabaseService
    {
        private static SQLiteAsyncConnection _db;
        static async Task Init()
        {
            if (_db != null)
            {
                return;
            }

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "studentPortal.db");
            _db = new SQLiteAsyncConnection(dbPath);

            await _db.CreateTableAsync<Term>();
            await _db.CreateTableAsync<Course>();
        }

        #region Terms region
        public static async Task AddTerm(string name, string startTime, string endTime)
        {
            await Init();
            var term = new Term
            {
                Name = name,
                StartTime = DateTime.Parse(startTime),
                EndTime = DateTime.Parse(endTime)
            };
            await _db.InsertAsync(term);
        }

        public static async Task RemoveTerm(int id)
        {
            await Init();
            var term = await _db.Table<Term>().Where(t => t.Id == id).FirstOrDefaultAsync();
            if (term != null)
            {
                await _db.DeleteAsync(term);
            }
        }

        public static async Task GetTerms()
        {
            await Init();
            var terms = await _db.Table<Term>().ToListAsync();
        }

        public static async Task UpdateTerm(int id, string name, string startTime, string endTime)
        {
            await Init();
            var term = await _db.Table<Term>().Where(t => t.Id == id).FirstOrDefaultAsync();
            if (term != null)
            {
                term.Name = name;
                term.StartTime = DateTime.Parse(startTime);
                term.EndTime = DateTime.Parse(endTime);
                await _db.UpdateAsync(term);
            }
        }

        #endregion


        #region Courses region
        public static async Task AddCourse(int termId, string name, string startTime, string endTime, string status,
            string instructorName, string instructorPhone, string instructorEmail, string notes,
            bool startNotification, bool endNotification, string dueDate)
        {
            await Init();
            var course = new Course
            {
                TermId = termId,
                Name = name,
                StartTime = DateTime.Parse(startTime),
                EndTime = DateTime.Parse(endTime),
                Status = status,
                InstructorName = instructorName,
                InstructorPhone = instructorPhone,
                InstructorEmail = instructorEmail,
                Notes = notes,
                StartNotification = startNotification,
                EndNotification = endNotification,
                DueDate = dueDate
            };
            await _db.InsertAsync(course);
        }

        public static async Task RemoveCourse(int id)
        {
            await Init();
            var course = await _db.Table<Course>().Where(c => c.Id == id).FirstOrDefaultAsync();
            if (course != null)
            {
                await _db.DeleteAsync(course);
            }
        }

        public static async Task GetCourses()
        {
            await Init();
            var courses = await _db.Table<Course>().ToListAsync();
        }

        public static async Task GetCoursesByTerm(int termId)
        {
            await Init();
            var courses = await _db.Table<Course>().Where(c => c.TermId == termId).ToListAsync();
        }

        public static async Task UpdateCourse(int id, int termId, string name, string startTime, string endTime, string status,
            string instructorName, string instructorPhone, string instructorEmail, string notes,
            bool startNotification, bool endNotification, string dueDate)
        {
            await Init();
            var course = await _db.Table<Course>().Where(c => c.Id == id).FirstOrDefaultAsync();
            if (course != null)
            {
                course.TermId = termId;
                course.Name = name;
                course.StartTime = DateTime.Parse(startTime);
                course.EndTime = DateTime.Parse(endTime);
                course.Status = status;
                course.InstructorName = instructorName;
                course.InstructorPhone = instructorPhone;
                course.InstructorEmail = instructorEmail;
                course.Notes = notes;
                course.StartNotification = startNotification;
                course.EndNotification = endNotification;
                course.DueDate = dueDate;
                await _db.UpdateAsync(course);
            }
        }

        #endregion

        #region DemoData
        public static async void LoadSampleData()
        {
            await Init();

            Term term1 = new Term
            {
                Name = "Fall 2025",
                StartTime = new DateTime(2025, 10, 19),
                EndTime = new DateTime(2026, 4, 19),
            };

            await _db.InsertAsync(term1);

            Course course1 = new Course
            {
                Name = "Introduction to C#",
                StartTime = new DateTime(2025, 10, 19),
                EndTime = new DateTime(2026, 11, 19),
                Status = "In Progress",
                InstructorName = "John Doe",
                InstructorPhone = "555-1234",
                InstructorEmail = "email@gmail.com",
                Notes = "Here are my notes",
                StartNotification = true,
                EndNotification = false,
                DueDate = "2025-11-19",
                TermId = term1.Id,
            };

            await _db.InsertAsync(course1);

            Course course2 = new Course
            {
                Name = "Database Systems",
                StartTime = new DateTime(2025, 11, 19),
                EndTime = new DateTime(2026, 12, 19),
                Status = "Not started",
                InstructorName = "Jane Smith",
                InstructorPhone = "555-5678",
                InstructorEmail = "email@gmail.com",
                Notes = "Sample notes",
                StartNotification = false,
                EndNotification = true,
                DueDate = "2025-12-19",
                TermId = term1.Id,
            };

            await _db.InsertAsync(course2);

            Course course3 = new Course
            {
                Name = "Mobile App Development",
                StartTime = new DateTime(2025, 12, 19),
                EndTime = new DateTime(2026, 1, 19),
                Status = "Not started",
                InstructorName = "Alice Johnson",
                InstructorPhone = "555-9012",
                InstructorEmail = "gmail@gmail.com",
                Notes = "Some notes here",
                StartNotification = true,
                EndNotification = true,
                DueDate = "2026-01-19",
                TermId = term1.Id,
            };
            await _db.InsertAsync(course3);

            Course course4 = new Course
            {
                Name = "Web Development",
                StartTime = new DateTime(2026, 1, 19),
                EndTime = new DateTime(2026, 2, 19),
                Status = "Not started",
                InstructorName = "Bob Brown",
                InstructorPhone = "555-3456",
                InstructorEmail = "email@gmail.com",
                Notes = "Web dev class",
                StartNotification = false,
                EndNotification = true,
                DueDate = "2026-02-19",
                TermId = term1.Id,
            };
            await _db.InsertAsync(course4);

            Course course5 = new Course
            {
                Name = "Data Structures",
                StartTime = new DateTime(2026, 2, 19),
                EndTime = new DateTime(2026, 3, 19),
                Status = "Not started",
                InstructorName = "Charlie Davis",
                InstructorPhone = "555-7890",
                InstructorEmail = "email@gmal.com",
                Notes = "Data structures class",
                StartNotification = true,
                EndNotification = true,
                DueDate = "2026-03-19",
                TermId = term1.Id,
            };
            await _db.InsertAsync(course5);

            Course course6 = new Course
            {
                Name = "Operating Systems",
                StartTime = new DateTime(2026, 3, 19),
                EndTime = new DateTime(2026, 4, 19),
                Status = "Not started",
                InstructorName = "Diana Evans",
                InstructorPhone = "555-2345",
                InstructorEmail = "gmail@gmail.com",
                Notes = "OS class notes",
                StartNotification = false,
                EndNotification = false,
                DueDate = "2026-04-19",
                TermId = term1.Id,
            };
            await _db.InsertAsync(course6);

            Term term2 = new Term
            {
                Name = "Spring 2026",
                StartTime = new DateTime(2026, 4, 19),
                EndTime = new DateTime(2026, 10, 19),
            };
            await _db.InsertAsync(term2);

            Term term3 = new Term
            {
                Name = "Summer 2026",
                StartTime = new DateTime(2026, 10, 19),
                EndTime = new DateTime(2027, 4, 19),
            };
            await _db.InsertAsync(term3);

        }

        public static async Task ClearSampleData()
        {
            await Init();
            await _db.DropTableAsync<Term>();
            await _db.DropTableAsync<Course>();
            _db = null;

            Settings.ClearSettings();
        }

        #endregion
    }
}

