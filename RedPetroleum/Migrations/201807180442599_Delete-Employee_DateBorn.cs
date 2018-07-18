namespace RedPetroleum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteEmployee_DateBorn : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employees", "AdoptionDate", c => c.String());
            DropColumn("dbo.Employees", "DateBorn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "DateBorn", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Employees", "AdoptionDate", c => c.DateTime(nullable: false));
        }
    }
}
