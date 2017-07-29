using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MinimercadoAlfredo.Models;

namespace MinimercadoAlfredo.ViewModels.ResumenVentas
{
    public class DaySalesVM
    {
        public DateTime Date { get; set; }

        public ICollection<Sale> Sales { get; set; }

        public decimal DaySalesTotal { get; set; }

        //public decimal DayGain { get; set; }
    }
}