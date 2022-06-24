using AutoMapper;
using Cms.Data.Repository.Models;
using Cms.Data.Repository.Repositories;
using Cms.WebApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Cms.WebApi.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("courses")]
    public class Courses2Controller : ControllerBase
    {
        private readonly ICmsRepository _cmsRepository;
        private readonly IMapper _mapper;

        public Courses2Controller(ICmsRepository cmsRepository, IMapper mapper)
        {
            _cmsRepository = cmsRepository;
            _mapper = mapper;
        }

        // Return type - Approach 1 - primitive or complex type
        //[HttpGet]
        //public IEnumerable<CourseDto> GetCourses()
        //{
        //    try
        //    {
        //        IEnumerable<Course> courses = _cmsRepository.GetAllCourses();
        //        var result = MapCourseToCourseDto(courses);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //// Return type - Approach 2 - IActionResult
        //[HttpGet]
        //public IActionResult GetCourses()
        //{
        //    try
        //    {
        //        IEnumerable<Course> courses = _cmsRepository.GetAllCourses();
        //        var result = MapCourseToCourseDto(courses);
        //        var foo = 0;
        //        var bar = 4 / foo;
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }
        //}

        // Return type - Approach 3 - ActionResult<T>
        //[HttpGet]
        //public ActionResult<IEnumerable<CourseDto>> GetCourses()
        //{
        //    try
        //    {
        //        IEnumerable<Course> courses = _cmsRepository.GetAllCourses();
        //        var result = MapCourseToCourseDto(courses);
        //        return result.ToList(); // Convert to support ActionResult<T>
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }
        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCoursesAsync()
        {
            try
            {
                IEnumerable<Course> courses = await _cmsRepository.GetAllCoursesAsync();
                //var result = MapCourseToCourseDto(courses);
                var result = _mapper.Map<CourseDto[]>(courses);

                // version 2 changes
                foreach (var item in result)
                {
                    item.CourseName += " (v2.0)";
                }
                return result.ToList(); // Convert to support ActionResult<T>
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<CourseDto> AddCourse([FromBody] CourseDto courseDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newCourse = _mapper.Map<Course>(courseDto);
                newCourse = _cmsRepository.AddCourse(newCourse);
                return _mapper.Map<CourseDto>(newCourse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{courseId}")]
        public ActionResult<CourseDto> GetCourse(int courseId)
        {
            try
            {
                if (!_cmsRepository.IsCourseExists(courseId))
                    return NotFound();

                Course course = _cmsRepository.GetCourse(courseId);
                var result = _mapper.Map<CourseDto>(course);
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{courseId}")]
        public ActionResult<CourseDto> UpdateCourse(int courseId, CourseDto courseDto)
        {
            try
            {
                if (!_cmsRepository.IsCourseExists(courseId))
                {
                    return NotFound();
                }

                Course updatedCourse = _mapper.Map<Course>(courseDto);
                updatedCourse = _cmsRepository.UpdateCourse(courseId, updatedCourse);
                var result = _mapper.Map<CourseDto>(updatedCourse);
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{courseId}")]
        public ActionResult<CourseDto> DeleteCourse(int courseId)
        {
            try
            {
                if (!_cmsRepository.IsCourseExists(courseId))
                    return NotFound();

                Course course = _cmsRepository.DeleteCourse(courseId);

                if (course == null)
                    return BadRequest();

                var result = _mapper.Map<CourseDto>(course);
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        // GET ../courses/1/students
        [HttpGet("{courseId}/students")]
        public ActionResult<IEnumerable<StudentDto>> GetStudents(int courseId)
        {
            try
            {
                if (!_cmsRepository.IsCourseExists(courseId))
                    return NotFound();

                IEnumerable<Student> students = _cmsRepository.GetStudents(courseId);
                var result = _mapper.Map<StudentDto[]>(students);
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // POST ../courses/1/students
        [HttpPost("{courseId}/students")]
        public ActionResult<StudentDto> AddStudent(int courseId, StudentDto studentDto)
        {
            try
            {
                if (!_cmsRepository.IsCourseExists(courseId))
                    return NotFound();

                Student student = _mapper.Map<Student>(studentDto);

                // Assign course
                Course course = _cmsRepository.GetCourse(courseId); 
                student.Course = course;

                student = _cmsRepository.AddStudent(student);
                
                var result = _mapper.Map<StudentDto>(student);
                
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }





        #region Custom mapper functions

        //private CourseDto MapCourseToCourseDto(Course course)
        //{
        //    return new CourseDto()
        //    {
        //        CourseId = course.CourseId,
        //        CourseName = course.CourseName,
        //        CourseDuration = course.CourseDuration,
        //        CourseType = (DTOs.COURSE_TYPE)course.CourseType
        //    };
        //}

        //private IEnumerable<CourseDto> MapCourseToCourseDto(IEnumerable<Course> courses)
        //{
        //    IEnumerable<CourseDto> result;

        //    result = courses.Select(c => new CourseDto()
        //    {
        //        CourseId = c.CourseId,
        //        CourseName = c.CourseName,
        //        CourseDuration = c.CourseDuration,
        //        CourseType = (DTOs.COURSE_TYPE)c.CourseType
        //    });

        //    return result;
        //}

        #endregion Custom mapper functions
    }
}