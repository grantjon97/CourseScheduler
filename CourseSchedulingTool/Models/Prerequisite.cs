using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseSchedulingTool.Models
{
    public class Prerequisite
    {
        public int Id { get; set; }
        public Course Course { get; set; }
        public Course CourseRequired { get; set; }
    }
}