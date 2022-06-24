using AutoMapper;
using Cms.Data.Repository.Models;
using Cms.Data.Repository.Repositories;
using Cms.WebApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Cms.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICmsRepository _cmsRepository;
        private readonly IMapper _mapper;

        public CoursesController(ICmsRepository cmsRepository, IMapper mapper)
        {
            _cmsRepository = cmsRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCoursesAsync()
        {
            try
            {
                IEnumerable<Course> courses = await _cmsRepository.GetAllCoursesAsync();
                var result = _mapper.Map<CourseDto[]>(courses);
                return result.ToList();
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
    }
}