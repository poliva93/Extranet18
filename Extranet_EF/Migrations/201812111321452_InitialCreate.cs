namespace Extranet_EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EDI_RIGHE",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ID_TESTATA = c.Int(nullable: false),
                        NUMORDINE = c.String(nullable: false, maxLength: 12, unicode: false),
                        ARTCOD = c.String(nullable: false, maxLength: 15, unicode: false),
                        ARTVER = c.String(maxLength: 5, unicode: false),
                        ARTDES = c.String(maxLength: 100, unicode: false),
                        ARTQTA = c.Decimal(nullable: false, precision: 10, scale: 3),
                        TIPOLOGIA = c.String(),
                        ARTUM = c.String(nullable: false, maxLength: 5, unicode: false),
                        DATA_CONSEGNA = c.Decimal(precision: 18, scale: 2),
                        LAST_DDT = c.String(maxLength: 30, unicode: false),
                        LAST_DDT_F = c.String(maxLength: 30, unicode: false),
                        LAST_DATA = c.DateTime(storeType: "date"),
                        LAST_QTA = c.Decimal(precision: 10, scale: 3),
                        PROG_ARTICOLO = c.Decimal(precision: 10, scale: 3),
                        Ordinamento = c.Int(nullable: false),
                        rank = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.EDI_TESTATA", t => new { t.ID_TESTATA, t.NUMORDINE })
                .Index(t => new { t.ID_TESTATA, t.NUMORDINE });
            
            CreateTable(
                "dbo.EDI_TESTATA",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        NUMORDINE = c.String(nullable: false, maxLength: 12, unicode: false),
                        CLFCOD = c.String(nullable: false, maxLength: 10, unicode: false),
                        DATAPIANO = c.DateTime(nullable: false, storeType: "date"),
                        CLFDES = c.String(nullable: false, maxLength: 50, unicode: false),
                        CLFIND = c.String(nullable: false, maxLength: 200, unicode: false),
                        MAGAZZINO = c.String(maxLength: 10, unicode: false),
                        CONTATTOLOG = c.String(maxLength: 100, unicode: false),
                        CONTATTOFOR = c.String(maxLength: 100, unicode: false),
                        DATAVIS = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.ID, t.NUMORDINE })
                .ForeignKey("dbo.FORNITORE", t => t.CLFCOD)
                .Index(t => t.CLFCOD);
            
            CreateTable(
                "dbo.FORNITORE",
                c => new
                    {
                        CLFCOD = c.String(nullable: false, maxLength: 10, unicode: false),
                    })
                .PrimaryKey(t => t.CLFCOD);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        CodiceFornitore = c.String(maxLength: 10, unicode: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        DOI = c.String(),
                        ActivationCode = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.FORNITORE", t => t.CodiceFornitore)
                .Index(t => t.CodiceFornitore);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.Shares",
                c => new
                    {
                        ShareID = c.Int(nullable: false, identity: true),
                        SharePath = c.String(),
                        Users_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.ShareID)
                .ForeignKey("dbo.Users", t => t.Users_UserId)
                .Index(t => t.Users_UserId);
            
            CreateTable(
                "dbo.ShareRoles",
                c => new
                    {
                        ShareId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ShareId, t.RoleId })
                .ForeignKey("dbo.Roles", t => t.ShareId, cascadeDelete: true)
                .ForeignKey("dbo.Shares", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.ShareId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        RoleId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "CodiceFornitore", "dbo.FORNITORE");
            DropForeignKey("dbo.Shares", "Users_UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.ShareRoles", "RoleId", "dbo.Shares");
            DropForeignKey("dbo.ShareRoles", "ShareId", "dbo.Roles");
            DropForeignKey("dbo.EDI_TESTATA", "CLFCOD", "dbo.FORNITORE");
            DropForeignKey("dbo.EDI_RIGHE", new[] { "ID_TESTATA", "NUMORDINE" }, "dbo.EDI_TESTATA");
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.ShareRoles", new[] { "RoleId" });
            DropIndex("dbo.ShareRoles", new[] { "ShareId" });
            DropIndex("dbo.Shares", new[] { "Users_UserId" });
            DropIndex("dbo.Users", new[] { "CodiceFornitore" });
            DropIndex("dbo.EDI_TESTATA", new[] { "CLFCOD" });
            DropIndex("dbo.EDI_RIGHE", new[] { "ID_TESTATA", "NUMORDINE" });
            DropTable("dbo.UserRoles");
            DropTable("dbo.ShareRoles");
            DropTable("dbo.Shares");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.FORNITORE");
            DropTable("dbo.EDI_TESTATA");
            DropTable("dbo.EDI_RIGHE");
        }
    }
}
