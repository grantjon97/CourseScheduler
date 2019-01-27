namespace CourseSchedulingTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeCourseTypeRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Courses", "Type", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Courses", "Type", c => c.String());
        }
    }
}
