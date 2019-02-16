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

        public IEnumerable<Prerequisite> GetCourses()
        {
            var requiredCourses = context.Requirements
                .Include(c => c.Major)
                .Include(c => c.Course)
                .Where(c => (c.Major.Id == 1) && (c.IsElective == false))
                .ToList();

            var prerequisites = context.Prerequisites
                .Include(p => p.Course)
                .Include(p => p.CourseRequired)
                .ToList();

            // Equivalent to a SQL inner join
            var requiredCoursesAndPrerequisites =
                (from prerequisite in prerequisites
                 join requiredCourse in requiredCourses on prerequisite.Course.Id equals requiredCourse.Course.Id
                 select new Prerequisite { Course = prerequisite.Course, CourseRequired = prerequisite.CourseRequired })
                 .ToList();

            var coursesWithNoPrerequisites = requiredCoursesAndPrerequisites.Where(c => c.CourseRequired == null).ToList();
            ScheduleCourses(new List<Prerequisite> { }, coursesWithNoPrerequisites);

            return requiredCoursesAndPrerequisites;
        }

        private void ScheduleCourses(List<Prerequisite> previousCourses, List<Prerequisite> currentCourses)
        {
            if (currentCourses.Count == 0)
                // TODO: return an object of type "Schedule"
                return;

            foreach (var currentCourse in currentCourses)
            {
                // if currentCourse has no prerequisites, schedule it as early as possible.

                // else:

                    // foreach previousCourse that is required before currentCourse:

                        // all previous courses have already been scheduled, so find the
                        // previousCourse that was scheduled the latest.

                    // schedule the currentCourse to be as soon as possible right after
                    // the latest previousCourse that was scheduled

                // nextCourses = FindNextNodes(currentCourses)
                // ScheduleCourses(currentCourses, nextCourses)
            }

        }
    }
}
