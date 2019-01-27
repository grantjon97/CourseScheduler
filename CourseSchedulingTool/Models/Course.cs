using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseSchedulingTool.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int CourseNumber { get; set; }
        public string Title { get; set; }
        public float Credits { get; set; }
    }
}