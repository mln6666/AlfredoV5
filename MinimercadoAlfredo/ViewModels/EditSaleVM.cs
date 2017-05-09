using System;
using System.Collections.Generic;
using MinimercadoAlfredo.Models;
using System.Linq;
using System.Web;

namespace MinimercadoAlfredo.ViewModels
{
    public class EditSaleVM
    {
        public int IdSale { get; set; }

        public int SaleCustomer { get; set; }

        //public DateTime SaleDate { get; set; }

        public List<SaleLine> SaleLines { get; set; }

        public decimal SaleTotal { get; set; }

        public string SaleComments { get; set; }

        public int SaleState { get; set; }
    }
}