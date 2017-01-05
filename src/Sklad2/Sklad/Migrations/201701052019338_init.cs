namespace Sklad.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Materials",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CreatedBy = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Kgs = c.Int(nullable: false),
                        Bags = c.Int(nullable: false),
                        ActionedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ActionedBy_Id = c.Int(nullable: false),
                        From_Id = c.Int(nullable: false),
                        Material_Id = c.Int(nullable: false),
                        To_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Workers", t => t.ActionedBy_Id, cascadeDelete: true)
                .ForeignKey("dbo.Stages", t => t.From_Id)
                .ForeignKey("dbo.Materials", t => t.Material_Id, cascadeDelete: true)
                .ForeignKey("dbo.Stages", t => t.To_Id)
                .Index(t => t.ActionedBy_Id)
                .Index(t => t.From_Id)
                .Index(t => t.Material_Id)
                .Index(t => t.To_Id);
            
            CreateTable(
                "dbo.Workers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CreatedBy = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Stages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CreatedBy = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PipelineRules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FromId = c.Int(nullable: false),
                        ToId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stages", t => t.FromId)
                .ForeignKey("dbo.Stages", t => t.ToId)
                .Index(t => t.FromId)
                .Index(t => t.ToId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CreatedBy = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PipelineRules", "ToId", "dbo.Stages");
            DropForeignKey("dbo.PipelineRules", "FromId", "dbo.Stages");
            DropForeignKey("dbo.Orders", "To_Id", "dbo.Stages");
            DropForeignKey("dbo.Orders", "Material_Id", "dbo.Materials");
            DropForeignKey("dbo.Orders", "From_Id", "dbo.Stages");
            DropForeignKey("dbo.Orders", "ActionedBy_Id", "dbo.Workers");
            DropIndex("dbo.PipelineRules", new[] { "ToId" });
            DropIndex("dbo.PipelineRules", new[] { "FromId" });
            DropIndex("dbo.Orders", new[] { "To_Id" });
            DropIndex("dbo.Orders", new[] { "Material_Id" });
            DropIndex("dbo.Orders", new[] { "From_Id" });
            DropIndex("dbo.Orders", new[] { "ActionedBy_Id" });
            DropTable("dbo.Users");
            DropTable("dbo.PipelineRules");
            DropTable("dbo.Stages");
            DropTable("dbo.Workers");
            DropTable("dbo.Orders");
            DropTable("dbo.Materials");
        }
    }
}
