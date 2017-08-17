namespace MinimercadoAlfredo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class phoneToString : DbMigration
    {
        public override void Down()
        {
            AlterColumn("dbo.Customers", "CustomerPhone", c => c.Int(nullable: true));
            AlterColumn("dbo.Providers", "ProviderPhone", c => c.Int(nullable: true));
        }

        public override void Up()
        {
            AlterColumn("dbo.Customers", "CustomerPhone", c => c.String());
            AlterColumn("dbo.Providers", "ProviderPhone", c => c.String());
            
        }
    }
}
