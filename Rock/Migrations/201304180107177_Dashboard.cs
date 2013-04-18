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
    public partial class Dashboard : RockMigration
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
                        EntityTypeId = c.Int(nullable: false),
                        StartColumn = c.Int(nullable: false),
                        StartRow = c.Int(nullable: false),
                        WidgetWidth = c.Int(nullable: false),
                        WidgetHeight = c.Int(nullable: false),
                        WidgetTitle = c.String(nullable: false),
                        WidgetDescription = c.String(nullable: false),
                        PersonId = c.Int(nullable: false),
                        IsSystem = c.Boolean(nullable: false),
                        MetricTypeId = c.Int(nullable: false),
                        MetricStartDate = c.DateTime(nullable: false),
                        MetricEndDate = c.DateTime(nullable: false),
                        Guid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EntityType", t => t.EntityTypeId)
                .ForeignKey("dbo.Person", t => t.PersonId)
                .ForeignKey("dbo.Metric", t => t.MetricTypeId)
                .Index(t => t.EntityTypeId)
                .Index(t => t.PersonId)
                .Index(t => t.MetricTypeId);
            
            CreateIndex( "dbo.Dashboard", "Guid", true );
        }
        
        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            DropIndex("dbo.Dashboard", new[] { "MetricTypeId" });
            DropIndex("dbo.Dashboard", new[] { "PersonId" });
            DropIndex("dbo.Dashboard", new[] { "EntityTypeId" });
            DropForeignKey("dbo.Dashboard", "MetricTypeId", "dbo.Metric");
            DropForeignKey("dbo.Dashboard", "PersonId", "dbo.Person");
            DropForeignKey("dbo.Dashboard", "EntityTypeId", "dbo.EntityType");
            DropTable("dbo.Dashboard");
        }
    }
}
