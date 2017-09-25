namespace MinimercadoAlfredo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImpresaNoImpresa : DbMigration
    {
        public override void Up()
        {
          
            AddColumn("dbo.Sales","Impresa",c => c.Boolean(nullable:true));
            
        
        }
        
        
    }
}
