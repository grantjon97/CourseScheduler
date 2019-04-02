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

        public IEnumerable<Course> GetCourses()
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
                 .OrderBy(c => c.Course.CourseNumber)
                 .ToList();

            var g = BuildGraph(requiredCoursesAndPrerequisites);

            var courseOrder = BFS(g);

            return courseOrder.Select(n => n.Course);
        }

        private Dag BuildGraph(List<Prerequisite> prerequisites)
        {
            var g = new Dag { Nodes = new List<Node>()};
            var coursesAdded = new List<Course>();

            foreach (var p in prerequisites)
            {
                if (!coursesAdded.Contains(p.Course))
                {
                    coursesAdded.Add(p.Course);
                    g.Nodes.Add(new Node { Course = p.Course, AdjacencyList = null });
                }
            }

            foreach (var node in g.Nodes)
            {
                var courseList = prerequisites.Where(c => c.CourseRequired == node.Course)
                    .Select(c => c.Course)
                    .OrderBy(c => c.CourseNumber)
                    .ToList();

                var adjacencyList = new List<Node>();
                foreach (var course in courseList)
                {
                    adjacencyList.Add(new Node { Course = course, AdjacencyList = { }, IsVisited = false });
                }

                node.AdjacencyList = adjacencyList;
            }

            return g;
        }

        private List<Node> BFS(Dag g)
        {
            var coursesVisited = new List<Node>();
            var nodeQueue = new List<Node>();
            var startingNode = FindStartingNode(g);

            SearchBFS(g, startingNode, nodeQueue, coursesVisited);

            while (nodeQueue.Count > 0)
            {
                var nodeRemoved = nodeQueue[0];
                nodeQueue.RemoveAt(0);
                if (nodeRemoved.IsVisited == false)
                {
                    SearchBFS(g, nodeRemoved, nodeQueue, coursesVisited);
                }
            }

            return coursesVisited;
        }

        private void SearchBFS(Dag g, Node v, List<Node> nodeQueue, List<Node> nodesVisited)
        {
            nodesVisited.Add(v);
            v.IsVisited = true;

            foreach (var node in v.AdjacencyList)
            {
                var nodeInMasterList = g.Nodes.First(c => c.Course == node.Course);
                nodeQueue.Add(nodeInMasterList);

                // This is a modified BFS that prefers to visit lower level classes first.
                nodeQueue.OrderBy(n => n.Course.CourseNumber);
            }
        }

        private Node FindStartingNode(Dag g)
        {
            var startingNode = g.Nodes[0];
            var found = false;
            var i = 0;
            while (!found)
            {
                // If the node we are looking at is not in any other nodes' adjacencyList,
                // then we know that this node is not required by any other node.
                if (NodeInAdjacencyList(g.Nodes, g.Nodes[i]) == false)
                {
                    startingNode = g.Nodes[i];
                    found = true;
                }
            }

            return startingNode;
        }

        private bool NodeInAdjacencyList(List<Node> nodes, Node nodeToCheck)
        {
            /*  Checks to see if a node is required by any other nodes.
             *  If a node is not required, then it will return false. 
             *  A node that is not required means it is a class with no prerequisites.
             */ 

            var inAdjacencyList = false;

            foreach (var node in nodes)
            {
                if (node.AdjacencyList.Contains(nodeToCheck))
                    inAdjacencyList = true;
            }

            return inAdjacencyList;
        }
    }
}
