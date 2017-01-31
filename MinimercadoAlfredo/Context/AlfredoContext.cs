using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MinimercadoAlfredo.Context
{
    public class AlfredoContext: DbContext

    {
        public AlfredoContext()
            : base("name=AlfredoContext")
        {
        }


        public System.Data.Entity.DbSet<MinimercadoAlfredo.Models.Product> Products { get; set; }

        public System.Data.Entity.DbSet<MinimercadoAlfredo.Models.Category> Categories { get; set; }

        public System.Data.Entity.DbSet<MinimercadoAlfredo.Models.Customer> Customers { get; set; }

        public System.Data.Entity.DbSet<MinimercadoAlfredo.Models.Provider> Providers { get; set; }

        public System.Data.Entity.DbSet<MinimercadoAlfredo.Models.Sale> Sales { get; set; }
        public System.Data.Entity.DbSet<MinimercadoAlfredo.Models.Bill> Bills { get; set; }


        public System.Data.Entity.DbSet<MinimercadoAlfredo.Models.Purchase> Purchases { get; set; }

        public System.Data.Entity.DbSet<MinimercadoAlfredo.Models.SaleLine> SaleLines { get; set; }
        public System.Data.Entity.DbSet<MinimercadoAlfredo.Models.BillLine> BillLines { get; set; }


        public System.Data.Entity.DbSet<MinimercadoAlfredo.Models.PurchaseLine> PurchaseLines { get; set; }
    }
}