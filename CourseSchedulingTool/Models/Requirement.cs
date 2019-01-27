using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseSchedulingTool.Models
{
    public class Requirement
    {
        public int Id { get; set; }
        public bool IsElective { get; set; }

        public Major Major { get; set; }
        public Course Course { get; set; }
    }
}