using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CourseSchedulingTool.Models;
using CourseSchedulingTool.ViewModels;

namespace CourseSchedulingTool.Conversions
{
    public static class ViewModels
    {
        public static CourseTermViewModel ConvertCourseTerm(CourseTerm courseTerm)
        {
            return new CourseTermViewModel
            {
                Id = courseTerm.Course.Id,
                CourseCode = courseTerm.Course.Type + courseTerm.Course.CourseNumber.ToString(),
                CourseTitle = courseTerm.Course.Title,
                Credits = courseTerm.Course.Credits,
                Year = courseTerm.Term.StartDate.Year,
                Season = courseTerm.Term.StartDate.Month == 8 ? "Fall" : "Spring"
            };
        }
    }
}