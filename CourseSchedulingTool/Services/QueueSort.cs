using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CourseSchedulingTool.Models;

namespace CourseSchedulingTool.Services
{
    public static class QueueSort
    {
        public static List<Node> Sort(ref List<Node> nodeQueue, bool sortByTime, bool sortByCourseNumber)
        {
            if (sortByTime && sortByCourseNumber)
                return ByTimeThenCourseNumber(ref nodeQueue);
            else if (sortByTime)
                return ByTime(ref nodeQueue);
            else
                return ByCourseNumber(ref nodeQueue);
        }

        private static List<Node> ByCourseNumber(ref List<Node> nodeQueue)
        {
            return nodeQueue.OrderBy(n => n.Course.CourseNumber).ToList();
        }

        private static List<Node> ByTime(ref List<Node> nodeQueue)
        {
            return nodeQueue.OrderBy(n => n.TermsOffered[0].StartDate).ToList();
        }

        private static List<Node> ByTimeThenCourseNumber(ref List<Node> nodeQueue)
        {
            return nodeQueue.OrderBy(n => n.TermsOffered[0].StartDate).ThenBy(n => n.Course.CourseNumber).ToList();
        }
    }
}