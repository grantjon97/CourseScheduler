namespace CourseSchedulingTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMajor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Majors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        MajorType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Majors");
        }
    }
}
