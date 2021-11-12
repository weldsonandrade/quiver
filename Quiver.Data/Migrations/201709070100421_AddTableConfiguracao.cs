namespace Quiver.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableConfiguracao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Configuracao",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 100),
                        Valor = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Nome, unique: true, name: "IX_Nome_Unique");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Configuracao", "IX_Nome_Unique");
            DropTable("dbo.Configuracao");
        }
    }
}
