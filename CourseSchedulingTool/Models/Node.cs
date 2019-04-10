using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseSchedulingTool.Models
{
    public class Node
    {
        public Course Course;
        public List<Term> TermsOffered;
        public List<Node> AdjacencyList;
        public bool IsVisited { get; set; } = false;
    }
}