using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using CourseSchedulingTool.Models;

namespace CourseSchedulingTool.Persistence
{
    public class SchedulerContext : DbContext
    {
        public SchedulerContext()
            : base("name=SchedulerContext")
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Major> Majors { get; set; }
        public DbSet<Term> Terms { get; set; }

        // Join tables
        public DbSet<Requirement> Requirements { get; set; }
        public DbSet<CourseTerm> CourseTerms { get; set; }
        public DbSet<Prerequisite> Prerequisites { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .Property(t => t.Title)
                .IsRequired();

            modelBuilder.Entity<Course>()
                .Property(t => t.Type)
                .IsRequired();

            modelBuilder.Entity<Major>()
                .Property(t => t.Title)
                .IsRequired();
        }
    }
}