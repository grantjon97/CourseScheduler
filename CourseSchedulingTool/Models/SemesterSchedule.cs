using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CourseSchedulingTool.Persistence;

namespace CourseSchedulingTool.Models
{
    public class SemesterSchedule
    {
        /*
         * This is made to be a javascript-friendly class. After a student selects their
         * major and clicks "create schedule," a list of "semesters" will be returned to be
         * displayed.
         */ 

        public Term Term { get; set; }
        public List<Course> Courses { get; set; }
    }

    public class Schedule : ApiController
    {
        public List<SemesterSchedule> SemesterSchedules { get; set; }
        private SchedulerContext context;

        public Schedule(List<Term> availableTerms)
        {
            /*
             * Class constructor -- Initialize the Schedule object with one semester, with no
             *                      classes scheduled in it so far.
             */
            SemesterSchedules.Add(new SemesterSchedule { Term = availableTerms[0], Courses = { } });

            context = new SchedulerContext();
        }

        public void AddCourse(Node node)
        {
            var termsCourseIsOffered = context.CourseTerms
                                                .Where(t => t.Course == node.Course)
                                                .Select(t => t.Term)
                                                .OrderBy(t => t.StartDate)
                                                .ToList();
            var done = false;
            var i = 0;
            while (!done && i < termsCourseIsOffered.Count)
            {
                if (CourseFitsInTerm(node, termsCourseIsOffered[i]))
                {
                    
                    done = true;
                }

                i++;
            }
        }

        private bool CourseFitsInTerm(Node node, Term term)
        {
            /*
             * Checks to see if a course can (and should be) scheduled within a given term.
             * 
             * Rules:
             * 1) The term being looked at must be after all terms that contain prerequisites.
             * 2) Adding the course cannot make the current term exceed 18 credits.
             */

            return true;
        }
    }
}