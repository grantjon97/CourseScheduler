using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using CourseSchedulingTool.Persistence;
using CourseSchedulingTool.Models;
using CourseSchedulingTool.Conversions;
using CourseSchedulingTool.ViewModels;

namespace CourseSchedulingTool.Controllers
{
    public class CoursesController : ApiController
    {
        private SchedulerContext context;

        public CoursesController()
        {
            context = new SchedulerContext();
        }

        public IEnumerable<CourseTermViewModel> GetCourses()
        {
            var courseTerms = context.CourseTerms
                .Include(c => c.Course)
                .Include(c => c.Term);

            var courseTermViews = new List<CourseTermViewModel>();

            foreach (var courseTerm in courseTerms){
                courseTermViews.Add(Conversions.ViewModels.ConvertCourseTerm(courseTerm));
            };

            return courseTermViews;
        }
    }
}
