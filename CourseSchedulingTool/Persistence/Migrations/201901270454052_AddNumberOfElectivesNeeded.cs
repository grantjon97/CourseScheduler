namespace CourseSchedulingTool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNumberOfElectivesNeeded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Majors", "NumberOfElectivesNeeded", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Majors", "NumberOfElectivesNeeded");
        }
    }
}
