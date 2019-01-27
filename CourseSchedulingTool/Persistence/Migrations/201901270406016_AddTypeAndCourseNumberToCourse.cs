namespace CourseSchedulingTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTypeAndCourseNumberToCourse : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "Type", c => c.String());
            AddColumn("dbo.Courses", "CourseNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "CourseNumber");
            DropColumn("dbo.Courses", "Type");
        }
    }
}
