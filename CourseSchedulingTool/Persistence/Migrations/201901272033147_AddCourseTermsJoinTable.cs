namespace CourseSchedulingTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCourseTermsJoinTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CourseTerms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Course_Id = c.Int(),
                        Term_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.Course_Id)
                .ForeignKey("dbo.Terms", t => t.Term_Id)
                .Index(t => t.Course_Id)
                .Index(t => t.Term_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CourseTerms", "Term_Id", "dbo.Terms");
            DropForeignKey("dbo.CourseTerms", "Course_Id", "dbo.Courses");
            DropIndex("dbo.CourseTerms", new[] { "Term_Id" });
            DropIndex("dbo.CourseTerms", new[] { "Course_Id" });
            DropTable("dbo.CourseTerms");
        }
    }
}
