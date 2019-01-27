using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseSchedulingTool.Models
{
    public class Major
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public MajorType MajorType { get; set; }
        public int NumberOfElectivesNeeded { get; set; }
    }

    public enum MajorType
    {
        BS,
        BA
    }
}