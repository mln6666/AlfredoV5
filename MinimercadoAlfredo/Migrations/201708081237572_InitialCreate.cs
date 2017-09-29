namespace MinimercadoAlfredo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BillLines",
                c => new
                    {
                        IdBillLine = c.Int(nullable: false, identity: true),
                        IdProduct = c.Int(nullable: false),
                        LinePrice = c.Decimal(nullable: false, precision: 18, scale: 3),
                        LineDiscount = c.Decimal(nullable: false, precision: 18, scale: 3),
                        LineQuantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        LineTotal = c.Decimal(nullable: false, precision: 18, scale: 3),
                        Return = c.Decimal(precision: 18, scale: 3),
                        LineTotalReturn = c.Decimal(nullable: false, precision: 18, scale: 3),
                        IdBill = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdBillLine)
                .ForeignKey("dbo.Bills", t => t.IdBill, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.IdProduct, cascadeDelete: true)
                .Index(t => t.IdProduct)
                .Index(t => t.IdBill);
            
            CreateTable(
                "dbo.Bills",
                c => new
                    {
                        IdBill = c.Int(nullable: false, identity: true),
                        SaleDate = c.DateTime(nullable: false),
                        SaleAddress = c.String(),
                        Discount = c.Int(),
                        Comments = c.String(),
                        SubTotal = c.Decimal(precision: 18, scale: 3),
                        LinesTotal = c.Decimal(precision: 18, scale: 3),
                        SaleTotal = c.Decimal(precision: 18, scale: 3),
                        IdCustomer = c.Int(),
                    })
                .PrimaryKey(t => t.IdBill)
                .ForeignKey("dbo.Customers", t => t.IdCustomer)
                .Index(t => t.IdCustomer);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        IdCustomer = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(nullable: false),
                        CustomerAddress = c.String(),
                        CustomerPhone = c.Int(),
                        CustomerEmail = c.String(),
                        CuitCuil = c.String(),
                    })
                .PrimaryKey(t => t.IdCustomer);
            
            CreateTable(
                "dbo.Sales",
                c => new
                    {
                        IdSale = c.Int(nullable: false, identity: true),
                        SaleDate = c.DateTime(nullable: false),
                        SaleAddress = c.String(),
                        Discount = c.Int(),
                        Comments = c.String(),
                        ReturnsTotal = c.Decimal(precision: 18, scale: 3),
                        LinesTotal = c.Decimal(precision: 18, scale: 3),
                        SubTotal = c.Decimal(precision: 18, scale: 3),
                        SaleTotal = c.Decimal(precision: 18, scale: 3),
                        IdCustomer = c.Int(),
                        IdBill = c.Int(),
                        SaleState = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdSale)
                .ForeignKey("dbo.Bills", t => t.IdBill)
                .ForeignKey("dbo.Customers", t => t.IdCustomer)
                .Index(t => t.IdCustomer)
                .Index(t => t.IdBill);
            
            CreateTable(
                "dbo.SaleLines",
                c => new
                    {
                        IdSaleLine = c.Int(nullable: false, identity: true),
                        IdProduct = c.Int(nullable: false),
                        LinePrice = c.Decimal(nullable: false, precision: 18, scale: 3),
                        LineDiscount = c.Decimal(nullable: false, precision: 18, scale: 3),
                        LineQuantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        LineTotal = c.Decimal(nullable: false, precision: 18, scale: 3),
                        Return = c.Decimal(precision: 18, scale: 3),
                        LineTotalReturn = c.Decimal(nullable: false, precision: 18, scale: 3),
                        IdSale = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdSaleLine)
                .ForeignKey("dbo.Products", t => t.IdProduct, cascadeDelete: true)
                .ForeignKey("dbo.Sales", t => t.IdSale, cascadeDelete: true)
                .Index(t => t.IdProduct)
                .Index(t => t.IdSale);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        IdProduct = c.Int(nullable: false, identity: true),
                        ProductDescription = c.String(nullable: false),
                        Cost = c.Decimal(precision: 18, scale: 3),
                        WholeSalePrice = c.Decimal(precision: 18, scale: 3),
                        UploadDate = c.DateTime(),
                        Stock = c.Decimal(precision: 18, scale: 3),
                        ParcialStock = c.Decimal(precision: 18, scale: 3),
                        Minimum = c.Decimal(precision: 18, scale: 3),
                        ProductState = c.Boolean(nullable: false),
                        idCategory = c.Int(nullable: false),
                        IdTrademark = c.Int(),
                    })
                .PrimaryKey(t => t.IdProduct)
                .ForeignKey("dbo.Categories", t => t.idCategory, cascadeDelete: true)
                .ForeignKey("dbo.Trademarks", t => t.IdTrademark)
                .Index(t => t.idCategory)
                .Index(t => t.IdTrademark);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        IdCategory = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false),
                        CategoryDescription = c.String(),
                    })
                .PrimaryKey(t => t.IdCategory);
            
            CreateTable(
                "dbo.ProductProviders",
                c => new
                    {
                        IdProductProvider = c.Int(nullable: false, identity: true),
                        IdProvider = c.Int(nullable: false),
                        IdProduct = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdProductProvider)
                .ForeignKey("dbo.Products", t => t.IdProduct, cascadeDelete: true)
                .ForeignKey("dbo.Providers", t => t.IdProvider, cascadeDelete: true)
                .Index(t => t.IdProvider)
                .Index(t => t.IdProduct);
            
            CreateTable(
                "dbo.Providers",
                c => new
                    {
                        IdProvider = c.Int(nullable: false, identity: true),
                        ProviderName = c.String(nullable: false),
                        CuitCuil = c.String(),
                        ProviderAddress = c.String(),
                        ProviderPhone = c.Int(),
                        ProviderEmail = c.String(),
                    })
                .PrimaryKey(t => t.IdProvider);
            
            CreateTable(
                "dbo.PurchaseLines",
                c => new
                    {
                        IdPurchaseLine = c.Int(nullable: false, identity: true),
                        IdProduct = c.Int(nullable: false),
                        LinePrice = c.Decimal(nullable: false, precision: 18, scale: 3),
                        LineQuantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        LineTotal = c.Decimal(nullable: false, precision: 18, scale: 3),
                        IdPurchase = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdPurchaseLine)
                .ForeignKey("dbo.Products", t => t.IdProduct, cascadeDelete: true)
                .ForeignKey("dbo.Purchases", t => t.IdPurchase, cascadeDelete: true)
                .Index(t => t.IdProduct)
                .Index(t => t.IdPurchase);
            
            CreateTable(
                "dbo.Purchases",
                c => new
                    {
                        IdPurchase = c.Int(nullable: false, identity: true),
                        PurchaseDate = c.DateTime(nullable: false),
                        Comments = c.String(),
                        PurchaseTotal = c.Decimal(precision: 18, scale: 3),
                        IdProvider = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdPurchase)
                .ForeignKey("dbo.Providers", t => t.IdProvider, cascadeDelete: true)
                .Index(t => t.IdProvider);
            
            CreateTable(
                "dbo.Trademarks",
                c => new
                    {
                        IdTrademark = c.Int(nullable: false, identity: true),
                        TrademarkName = c.String(nullable: false),
                        TrademarkDescription = c.String(),
                    })
                .PrimaryKey(t => t.IdTrademark);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SaleLines", "IdSale", "dbo.Sales");
            DropForeignKey("dbo.Products", "IdTrademark", "dbo.Trademarks");
            DropForeignKey("dbo.SaleLines", "IdProduct", "dbo.Products");
            DropForeignKey("dbo.PurchaseLines", "IdPurchase", "dbo.Purchases");
            DropForeignKey("dbo.Purchases", "IdProvider", "dbo.Providers");
            DropForeignKey("dbo.PurchaseLines", "IdProduct", "dbo.Products");
            DropForeignKey("dbo.ProductProviders", "IdProvider", "dbo.Providers");
            DropForeignKey("dbo.ProductProviders", "IdProduct", "dbo.Products");
            DropForeignKey("dbo.Products", "idCategory", "dbo.Categories");
            DropForeignKey("dbo.BillLines", "IdProduct", "dbo.Products");
            DropForeignKey("dbo.Sales", "IdCustomer", "dbo.Customers");
            DropForeignKey("dbo.Sales", "IdBill", "dbo.Bills");
            DropForeignKey("dbo.Bills", "IdCustomer", "dbo.Customers");
            DropForeignKey("dbo.BillLines", "IdBill", "dbo.Bills");
            DropIndex("dbo.Purchases", new[] { "IdProvider" });
            DropIndex("dbo.PurchaseLines", new[] { "IdPurchase" });
            DropIndex("dbo.PurchaseLines", new[] { "IdProduct" });
            DropIndex("dbo.ProductProviders", new[] { "IdProduct" });
            DropIndex("dbo.ProductProviders", new[] { "IdProvider" });
            DropIndex("dbo.Products", new[] { "IdTrademark" });
            DropIndex("dbo.Products", new[] { "idCategory" });
            DropIndex("dbo.SaleLines", new[] { "IdSale" });
            DropIndex("dbo.SaleLines", new[] { "IdProduct" });
            DropIndex("dbo.Sales", new[] { "IdBill" });
            DropIndex("dbo.Sales", new[] { "IdCustomer" });
            DropIndex("dbo.Bills", new[] { "IdCustomer" });
            DropIndex("dbo.BillLines", new[] { "IdBill" });
            DropIndex("dbo.BillLines", new[] { "IdProduct" });
            DropTable("dbo.Trademarks");
            DropTable("dbo.Purchases");
            DropTable("dbo.PurchaseLines");
            DropTable("dbo.Providers");
            DropTable("dbo.ProductProviders");
            DropTable("dbo.Categories");
            DropTable("dbo.Products");
            DropTable("dbo.SaleLines");
            DropTable("dbo.Sales");
            DropTable("dbo.Customers");
            DropTable("dbo.Bills");
            DropTable("dbo.BillLines");
        }
    }
}
