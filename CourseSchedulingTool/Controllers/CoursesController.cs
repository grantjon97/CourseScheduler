﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using CourseSchedulingTool.Persistence;
using CourseSchedulingTool.Models;

namespace CourseSchedulingTool.Controllers
{
    public class CoursesController : ApiController
    {
        private readonly SchedulerContext context;

        public IEnumerable<Course> GetCourses()
        {
            var courses = context.Courses.ToList();

            return courses;
        }
    }
}