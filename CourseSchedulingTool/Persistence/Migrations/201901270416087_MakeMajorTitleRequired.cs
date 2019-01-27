namespace CourseSchedulingTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeMajorTitleRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Majors", "Title", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Majors", "Title", c => c.String());
        }
    }
}
