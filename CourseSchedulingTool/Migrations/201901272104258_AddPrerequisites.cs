namespace CourseSchedulingTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPrerequisites : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Prerequisites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Course_Id = c.Int(),
                        CourseRequired_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.Course_Id)
                .ForeignKey("dbo.Courses", t => t.CourseRequired_Id)
                .Index(t => t.Course_Id)
                .Index(t => t.CourseRequired_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Prerequisites", "CourseRequired_Id", "dbo.Courses");
            DropForeignKey("dbo.Prerequisites", "Course_Id", "dbo.Courses");
            DropIndex("dbo.Prerequisites", new[] { "CourseRequired_Id" });
            DropIndex("dbo.Prerequisites", new[] { "Course_Id" });
            DropTable("dbo.Prerequisites");
        }
    }
}
