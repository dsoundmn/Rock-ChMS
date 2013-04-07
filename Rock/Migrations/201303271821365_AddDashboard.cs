//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
namespace Rock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    /// <summary>
    /// 
    /// </summary>
    public partial class AddDashboard : RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            CreateTable(
                "dbo.Dashboard",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsSystem = c.Boolean(nullable: false),
                        MetricId = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                        Description = c.String(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Size = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        Guid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Metric", t => t.MetricId, cascadeDelete: true)
                .ForeignKey("dbo.Person", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.MetricId)
                .Index(t => t.PersonId);
            
            CreateIndex( "dbo.Dashboard", "Guid", true );
        }
        
        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            DropIndex("dbo.Dashboard", new[] { "PersonId" });
            DropIndex("dbo.Dashboard", new[] { "MetricId" });
            DropForeignKey("dbo.Dashboard", "PersonId", "dbo.Person");
            DropForeignKey("dbo.Dashboard", "MetricId", "dbo.Metric");
            DropTable("dbo.Dashboard");
        }
    }
}
