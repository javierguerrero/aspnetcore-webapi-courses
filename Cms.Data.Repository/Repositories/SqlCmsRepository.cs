using Cms.Data.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cms.Data.Repository.Repositories
{
    public class SqlCmsRepository : ICmsRepository
    {
        public Course AddCourse(Course course)
        {
            throw new NotImplementedException();
        }

        public Student AddStudent(Student student)
        {
            throw new NotImplementedException();
        }

        public Course DeleteCourse(int courseId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Course> GetAllCourses()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            throw new NotImplementedException();
        }

        public Course GetCourse(int courseId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Student> GetStudents(int courseId)
        {
            throw new NotImplementedException();
        }

        public bool IsCourseExists(int courseId)
        {
            throw new NotImplementedException();
        }

        public Course UpdateCourse(int courseId, Course newCourse)
        {
            throw new NotImplementedException();
        }
    }
}
