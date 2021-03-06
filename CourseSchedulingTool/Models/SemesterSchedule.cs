﻿using System;
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
        private readonly List<Node> orderOfCourses;
        private readonly SchedulerContext context;

        public Schedule(List<Term> availableTerms, List<Node> _orderOfCourses)
        {
            /*
             * Class constructor -- Initialize the Schedule object with one semester, with no
             *                      classes scheduled in it so far.
             */
            SemesterSchedules = new List<SemesterSchedule> { };

            orderOfCourses = _orderOfCourses;
            context = new SchedulerContext();
        }

        public void AddCourse(Node node)
        {
            var termsCourseIsOffered = node.TermsOffered
                                           .OrderBy(t => t.StartDate)
                                           .ToList();

            var done = false;
            var i = 0;
            while (!done && i < termsCourseIsOffered.Count)
            {
                var termIndex = SemesterSchedules.FindIndex(s => s.Term.Id == termsCourseIsOffered[i].Id);

                // If the Schedule object has not created that term yet,
                // create it and then add the class to that term.
                if (termIndex < 0)
                {
                    SemesterSchedules.Add(new SemesterSchedule { Term = termsCourseIsOffered[i], Courses = new List<Course> { node.Course } });
                    done = true;
                }
                else
                {

                    if (CourseFitsInTerm(node, termsCourseIsOffered[i]))
                    {
                        SemesterSchedules[termIndex].Courses.Add(node.Course);
                        done = true;
                    }
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
             * 2) Adding the course cannot make the current term exceed 9 credits.
             */

            // Check that adding the course would not overload credits
            float creditCount = node.Course.Credits;
            foreach (var course in SemesterSchedules[SemesterSchedules.FindIndex(t => t.Term.Id == term.Id)].Courses)
                creditCount += course.Credits;
            if (creditCount > 9)
                return false;

            // Gather all courses that the current course requires.
            var prerequisites = context.Prerequisites
                                       .Where(p => p.Course.Id == node.Course.Id)
                                       .Select(p => p.CourseRequired)
                                       .ToList();

            if (prerequisites.Count > 0)
                return PrerequisitesHaveBeenTaken(node, prerequisites, term);

            // Classes with no prerequisites that do not overload the semester will
            // automatically fit into that semester schedule.
            return true;
        }

        private bool PrerequisitesHaveBeenTaken(Node node, List<Course> prerequisites, Term term)
        {
            /* 
             * Check that the course would be taken after all of its prerequisites have been taken
             * If this method is called, then we know that the course has at least 1 prerequisite.
             * 
             * prerequisiteCount - Counts how many prerequisites have been taken before the desired course
             *                     that is being scheduled. If prerequisiteCount is the same size as the
             *                     current course's adjacency list, then all prerequisites have been accounted for.
             */

            // The BFS returned an order of courses based off class level. The order returned by the BFS may reflect some
            // pseudo-prerequisites. For example, CS 348 can be taken directly after CS 131, but the BFS will push
            // CS 348 back to be taken after some lower level classes. Here, check that the course is not being taken
            // before any classes in the order returned by the BFS, whether or not they're actually prerequisites.
            var prerequisiteCount = 0;
            //var orderOfCoursesBefore = orderOfCourses.Take(orderOfCourses.FindIndex(n => n.Course.Id == node.Course.Id)).ToList();
            //foreach (var _node in orderOfCoursesBefore)
            //{
            //    var semestersUpToThisTerm = SemesterSchedules.Where(t => t.Term.StartDate <= term.StartDate);

            //    foreach (var semesterBefore in semestersUpToThisTerm)
            //    {
            //        if (semesterBefore.Courses.Any(c => c.Id == _node.Course.Id))
            //            prerequisiteCount++;
            //    }
            //}
            //if (prerequisiteCount != orderOfCoursesBefore.Count)
            //    return false;

            prerequisiteCount = 0;
            foreach (var prerequite in prerequisites)
            {
                var semestersBeforeThisTerm = SemesterSchedules.Where(t => t.Term.StartDate < term.StartDate);

                foreach (var semesterBefore in semestersBeforeThisTerm)
                {
                    if (semesterBefore.Courses.Any(c => c.Id == prerequite.Id))
                        prerequisiteCount++;
                }
            }
            if (prerequisiteCount == prerequisites.Count)
                return true;

            return false;
        }
    }
}