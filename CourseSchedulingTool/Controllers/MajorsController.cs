using CourseSchedulingTool.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CourseSchedulingTool.Models;

namespace CourseSchedulingTool.Controllers
{
    public class MajorsController : ApiController
    {
        private SchedulerContext context;

        public MajorsController()
        {
            context = new SchedulerContext();
        }

        public IHttpActionResult GetMajors()
        {
            var majors = context.Majors.ToList();

            var formattedMajors = majors
                .Select(m => new { id = m.Id,
                                   title = m.Title + " (" + Enum.GetName(typeof(MajorType), m.MajorType) + ")" })
                .ToList();
              
            return Ok(formattedMajors);
        }

        public IHttpActionResult GetElectives(int id)
        {
            var electives = context.Requirements
                .Where(r => r.Major.Id == id && r.IsElective == true)
                .Select(r => r.Course)
                .ToList();

            var numberOfElectivesNeeded = context.Majors
                .FirstOrDefault(m => m.Id == id).NumberOfElectivesNeeded;

            // Return something that looks similar to a course schedule object,
            // so that javascript can build a table for electives too.
            return Ok(new { numberOfElectivesNeeded, courses = electives });

        }
    }
}
