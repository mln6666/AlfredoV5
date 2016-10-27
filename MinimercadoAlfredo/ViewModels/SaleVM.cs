using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MinimercadoAlfredo.Models;

namespace MinimercadoAlfredo.ViewModels
{
    public class SaleVM
    {
        public int IdSale { get; set; }

        public string CustomerName { get; set; }

        public string CustomerAddress { get; set; }

        public string CuitCuil { get; set; }

        public string SaleAddress { get; set; }

        public DateTime SaleDate { get; set; }

        public List<SaleLine> SaleLines { get; set; }

        public decimal SaleTotal { get; set; }

        public string Comments { get; set; }


    }
}