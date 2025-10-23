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
    }
}
