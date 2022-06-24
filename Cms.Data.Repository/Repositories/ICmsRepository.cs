using Cms.Data.Repository.Models;
using System.Collections.Generic;

namespace Cms.Data.Repository.Repositories
{
    public interface ICmsRepository
    {
        // Collection
        IEnumerable<Course> GetAllCourses();

        Task<IEnumerable<Course>> GetAllCoursesAsync();


        // Individual item
        Course AddCourse(Course course);

        bool IsCourseExists(int courseId);
        Course GetCourse(int courseId);

        Course UpdateCourse(int courseId, Course newCourse);

        Course DeleteCourse(int courseId);

        // Association
        IEnumerable<Student> GetStudents(int courseId);

        Student AddStudent(Student student);
    }
}