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

            await _db.CreateTableAsync<User>();
            await _db.CreateTableAsync<Term>();
            await _db.CreateTableAsync<Course>();
            await _db.CreateTableAsync<Assessment>();
        }

        #region Users
        public static async Task<bool> AddUser(string username, string password)
        {
            await Init();
            // see if user exists
            var exists = await GetUser(username);
            if (exists != null)
            {
                // user exists
                return false;
            }
            var newUser = new User
            {
                UserName = username,
                Password = password
            };
            await _db.InsertAsync(newUser);
            return true;
        }

        public static async Task<User> GetUser(string username)
        {
            await Init();
            var user = await _db.Table<User>().Where(u => u.UserName == username).FirstOrDefaultAsync();
            // will return null if user doesn't exist
            return user;
        }

        public static async Task<bool> VerifyPassword(User user, string password)
        {
            await Init();
            if (user.Password == password)
            {
                // passwords match
                return true;
            }
            return false;
        }

        public static async Task<bool> UpdateUser(User user)
        {
            await Init();
            // see if username exists, if not return false
            var exists = await GetUser(user.UserName);
            if (exists == null)
            {
                await _db.UpdateAsync(user);
                return true;
            }
            return false;
        }

        public static async Task DeleteUser(User user)
        {
            await Init();
            await _db.DeleteAsync(user);
        }

        #endregion Users

        #region Terms region
        public static async Task AddTerm(int userId, string name, DateTime startTime, DateTime endTime)
        {
            await Init();
            var term = new Term
            {
                UserId = userId,
                Name = name,
                StartTime = startTime,
                EndTime = endTime
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

        public static async Task<List<Term>> GetTerms(int uid)
        {
            await Init();
            var terms = await _db.Table<Term>().Where(u => u.UserId == uid).ToListAsync();
            return terms;
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
                DueDate = DateTime.Parse(dueDate)
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

        public static async Task<List<Course>> GetCourses()
        {
            await Init();
            var courses = await _db.Table<Course>().ToListAsync();
            return courses;
        }

        public static async Task<List<Course>> GetCoursesByTerm(int termId)
        {
            await Init();
            var courses = await _db.Table<Course>().Where(c => c.TermId == termId).ToListAsync();
            return courses;
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
                course.DueDate = DateTime.Parse(dueDate);
                await _db.UpdateAsync(course);
            }
        }

        #endregion

        #region Assessments region
        public static async Task AddAssessment(int courseId, string type, string name, string start, string end, string status, bool startNotification, bool endNotification)
        {
            await Init();
            var assessment = new Assessment
            {
                CourseId = courseId,
                Type = type,
                Name = name,
                StartTime = DateTime.Parse(start),
                EndTime = DateTime.Parse(end),
                Status = status,
                StartNotification = startNotification,
                EndNotification = endNotification
            };
            await _db.InsertAsync(assessment);
        }

        public static async Task RemoveAssessment(int id)
        {
            await Init();
            var assessment = await _db.Table<Assessment>().Where(a => a.Id == id).FirstOrDefaultAsync();
            if (assessment != null)
            {
                await _db.DeleteAsync(assessment);
            }
        }

        public static async Task<List<Assessment>> GetAssessments(int courseId)
        {
            await Init();
            var assessments = await _db.Table<Assessment>().Where(a => a.CourseId == courseId).ToListAsync();
            return assessments;
        }

        public static async Task<Assessment> GetAssessmentByType(int courseId, string type)
        {
            await Init();
            var assessment = await _db.Table<Assessment>().Where(a => a.CourseId == courseId && a.Type == type).FirstOrDefaultAsync();
            return assessment;
        }

        public static async Task UpdateAssessment(int id, int courseId, string type, string name, string start, string end, string status, bool startNotification, bool endNotification)
        {
            await Init();
            var assessment = await _db.Table<Assessment>().Where(a => a.Id == id).FirstOrDefaultAsync();
            if (assessment != null)
            {
                assessment.CourseId = courseId;
                assessment.Type = type;
                assessment.Name = name;
                assessment.StartTime = DateTime.Parse(start);
                assessment.EndTime = DateTime.Parse(end);
                assessment.Status = status;
                assessment.StartNotification = startNotification;
                assessment.EndNotification = endNotification;
                await _db.UpdateAsync(assessment);
            }
        }
        #endregion

        #region DemoData
        public static async Task LoadSampleData()
        {
            await Init();

            User user1 = new User
            {
                UserName = "user",
                Password = "password",
            };

            await _db.InsertAsync(user1);

            Term term1 = new Term
            {
                UserId = user1.Id,
                Name = "Fall 2025",
                StartTime = new DateTime(2025, 10, 19),
                EndTime = new DateTime(2026, 4, 19),
            };

            await _db.InsertAsync(term1);

            Course course1 = new Course
            {
                Name = "Mobile App development in C#",
                StartTime = new DateTime(2025, 10, 19),
                EndTime = new DateTime(2025, 11, 19),
                Status = "In Progress",
                InstructorName = "Anika Patel",
                InstructorPhone = "555-123-4567",
                InstructorEmail = "anika.patel@strimeuniversity.edu",
                Notes = "Here are my notes",
                StartNotification = true,
                EndNotification = false,
                DueDate = new DateTime(2026, 11, 19),
                TermId = term1.Id,
            };

            await _db.InsertAsync(course1);

            Assessment assessment1 = new Assessment
            {
                CourseId = course1.Id,
                Type = "Objective Assessment",
                Name = "Final Test",
                StartTime = new DateTime(2025, 10, 26),
                EndTime = new DateTime(2025, 11, 2),
                Status = "In Progress",
                StartNotification = false,
                EndNotification = false,
            };

            await _db.InsertAsync(assessment1);

            Assessment assessment2 = new Assessment
            {
                CourseId = course1.Id,
                Type = "Performance Assessment",
                Name = "Project",
                StartTime = new DateTime(2025, 11, 9),
                EndTime = new DateTime(2025, 11, 16),
                Status = "Planned",
                StartNotification = false,
                EndNotification = false,
            };

            await _db.InsertAsync(assessment2);

            
        }

        public static async Task ClearSampleData()
        {
            await Init();
            await _db.DropTableAsync<User>();
            await _db.DropTableAsync<Term>();
            await _db.DropTableAsync<Course>();
            await _db.DropTableAsync<Assessment>();
            _db = null;

            Settings.ClearSettings();
        }

        #endregion
    }
}

