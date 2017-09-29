namespace MinimercadoAlfredo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class impresa : DbMigration
    {
        public override void Up()
        {
         
            AlterColumn("dbo.Customers", "CustomerPhone", c => c.String());
            AlterColumn("dbo.Providers", "ProviderPhone", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Providers", "ProviderPhone", c => c.Int());
            AlterColumn("dbo.Customers", "CustomerPhone", c => c.Int());
         
        }
    }
}
