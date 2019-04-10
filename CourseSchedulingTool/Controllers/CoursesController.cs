using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using CourseSchedulingTool.Persistence;
using CourseSchedulingTool.Models;
using CourseSchedulingTool.Services;
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

            return BFS(g, sortByCourseNumber: true, sortByTime: false);
        }

        private Dag BuildGraph(List<Prerequisite> prerequisites)
        {
            /*
             *  Builds a graph structure out of a list of courses in the database.
             *  The prerequisites argument is a list of pairs of classes (c1, c2) where c1 requires c2 to be taken first.
             *  The graph is directed and acyclic.
             *  
             *  The resulting DAG is just a list of nodes, where each node has its own adjacency list (edges)
             */ 

            var g = new Dag { Nodes = new List<Node>()};
            var coursesAdded = new List<Course>();

            // Create new nodes for each class that was stored in the DB list.
            // Each node will currently have a blank adjacency list.
            foreach (var p in prerequisites)
            {
                if (!coursesAdded.Contains(p.Course))
                {
                    coursesAdded.Add(p.Course);
                    var termsOffered = context.CourseTerms
                                              .Where(c => c.Course.Id == p.Course.Id)
                                              .Include(c => c.Term)
                                              .Select(c => c.Term)
                                              .OrderBy(t => t.StartDate)
                                              .ToList();
                    g.Nodes.Add(new Node { Course = p.Course, AdjacencyList = null, TermsOffered = termsOffered });
                }
            }

            // Create the edges of the graph.
            // (Each node's adjacency list is filled)
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

        private List<Course> BFS(Dag g, bool sortByCourseNumber, bool sortByTime)
        {
            /*
             *  Perform a BFS on a graph.
             *  
             *  sortByCourseNumber: When true, schedule the lowest-level classes first.
             *  sortByTime:         When true, schedule classes as early as possible.
             *  
             *  If both sorts are true, first find the classes that are offered earliest, and then
             *  from those, pick the ones that have the lowest course number. This should be the
             *  ideal way to perform the BFS.
             */
            var coursesVisited = new List<Node>();
            var nodeQueue = new List<Node>();
            var startingNode = FindStartingNode(g);

            SearchBFS(g, startingNode, ref nodeQueue, coursesVisited, sortByCourseNumber, sortByTime);

            while (nodeQueue.Count > 0)
            {
                var nodeRemoved = nodeQueue[0];
                nodeQueue.RemoveAt(0);
                if (nodeRemoved.IsVisited == false)
                {
                    SearchBFS(g, nodeRemoved, ref nodeQueue, coursesVisited, sortByCourseNumber, sortByTime);
                }
            }

            return coursesVisited.Select(c => c.Course).ToList();
        }

        private void SearchBFS(Dag g, Node v, ref List<Node> nodeQueue, 
                               List<Node> nodesVisited, bool sortByCourseNumber, bool sortByTime)
        {
            /*
             *  Visits a node v, and adds all of its adjacent nodes to a queue to be visited.
             *  Maintains a specific order to the queue, depending on if we want to sort the queue
             *  by date offered, course number, or both.
             */

            nodesVisited.Add(v);
            v.IsVisited = true;

            // Schedule the course
            // schedule.addCourse(v);

            foreach (var node in v.AdjacencyList)
            {
                var nodeInMasterList = g.Nodes.First(c => c.Course == node.Course);
                nodeQueue.Add(nodeInMasterList);
            }

            // This is where the original BFS algorithm is modified to prefer certain courses first.
            nodeQueue = QueueSort.Sort(ref nodeQueue, sortByTime, sortByCourseNumber);
        }

        private Node FindStartingNode(Dag g)
        {
            /*
             *  Finds a starting node for the modified BFS algorithm.
             *  The starting node (i.e. course) must not have any prerequisites.
             */ 

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
             *  This function is used to find a starting node for the BFS.
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
