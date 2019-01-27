namespace CourseSchedulingTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequirements : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Requirements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsElective = c.Boolean(nullable: false),
                        Course_Id = c.Int(),
                        Major_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.Course_Id)
                .ForeignKey("dbo.Majors", t => t.Major_Id)
                .Index(t => t.Course_Id)
                .Index(t => t.Major_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requirements", "Major_Id", "dbo.Majors");
            DropForeignKey("dbo.Requirements", "Course_Id", "dbo.Courses");
            DropIndex("dbo.Requirements", new[] { "Major_Id" });
            DropIndex("dbo.Requirements", new[] { "Course_Id" });
            DropTable("dbo.Requirements");
        }
    }
}
