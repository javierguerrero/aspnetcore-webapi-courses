using Cms.Data.Repository.Models;

namespace Cms.Data.Repository.Repositories
{
    public class InMemoryCmsRepository : ICmsRepository
    {
        private List<Course> courses = new List<Course>();
        private List<Student> students = new List<Student>();

        public InMemoryCmsRepository()
        {
            courses.Add(new Course()
            {
                CourseId = 1,
                CourseName = "Computer Science",
                CourseDuration = 4,
                CourseType = COURSE_TYPE.ENGINEERING
            });
            courses.Add(new Course()
            {
                CourseId = 2,
                CourseName = "Information Technology",
                CourseDuration = 4,
                CourseType = COURSE_TYPE.ENGINEERING
            });

            students.Add(new Student()
            {
                StudentId = 101,
                FirstName = "James",
                LastName = "Smith",
                PhoneNumber = "555-555-1234",
                Address = "US",
                Course = courses.Where(c => c.CourseId == 1).SingleOrDefault()
            });
            students.Add(new Student()
            {
                StudentId = 102,
                FirstName = "Robert",
                LastName = "Smith",
                PhoneNumber = "555-555-5678",
                Address = "Canada",
                Course = courses.Where(c => c.CourseId == 1).SingleOrDefault()
            });
        }

        public Course AddCourse(Course course)
        {
            var maxCourseId = courses.Max(c => c.CourseId);
            course.CourseId = maxCourseId + 1;
            courses.Add(course);
            return course;
        }

        public Student AddStudent(Student newStudent)
        {
            var maxStudentId = students.Max(c => c.StudentId);
            newStudent.StudentId = maxStudentId + 1;
            students.Add(newStudent);

            return newStudent;
        }

        public Course DeleteCourse(int courseId)
        {
            var course = courses.Where(c => c.CourseId == courseId).SingleOrDefault();
            if (course != null)
                courses.Remove(course);

            return course;
        }

        public IEnumerable<Course> GetAllCourses()
        {
            return courses;
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await Task.Run(() => courses.ToList());
        }

        public Course GetCourse(int courseId)
        {
            var result = courses.Where(c => c.CourseId == courseId).SingleOrDefault();
            return result;
        }

        public IEnumerable<Student> GetStudents(int courseId)
        {
            return students.Where(c => c.Course.CourseId == courseId);
        }

        public bool IsCourseExists(int courseId)
        {
            return courses.Any(c => c.CourseId == courseId);
        }

        public Course UpdateCourse(int courseId, Course updatedCourse)
        {
            var course = courses.Where(c => c.CourseId == courseId).SingleOrDefault();

            if (course != null)
            {
                course.CourseName = updatedCourse.CourseName;
                course.CourseDuration = updatedCourse.CourseDuration;
                course.CourseType = updatedCourse.CourseType;
            }
            return course;
        }
    }
}