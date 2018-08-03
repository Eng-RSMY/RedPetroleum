namespace RedPetroleum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Visitation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Visitations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        EmployeeId = c.Guid(),
                        Date = c.DateTime(nullable: false),
                        VisitMark = c.Double(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId)
                .Index(t => t.EmployeeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Visitations", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.Visitations", new[] { "EmployeeId" });
            DropTable("dbo.Visitations");
        }
    }
}
