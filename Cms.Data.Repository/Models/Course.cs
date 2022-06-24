using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cms.Data.Repository.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int CourseDuration { get; set; }
        public COURSE_TYPE CourseType { get; set; }
    }

    public enum COURSE_TYPE
    {
        ENGINEERING, 
        MEDICAL,
        MANAGEMENT
    }
}
