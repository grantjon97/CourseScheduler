using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseSchedulingTool.ViewModels
{
    public class CourseTermViewModel
    {
        public int Id { get; set; }
        public string CourseCode { get; set; }
        public string CourseTitle { get; set; }
        public float Credits { get; set; }
        public string Season { get; set; }
        public int Year { get; set; }
    }
}