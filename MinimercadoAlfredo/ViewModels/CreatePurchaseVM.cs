using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MinimercadoAlfredo.Models;

namespace MinimercadoAlfredo.ViewModels
{
    public class CreatePurchaseVM
    {
        public Purchase Purchase { get; set; }

        public ICollection<Provider> Providers { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}