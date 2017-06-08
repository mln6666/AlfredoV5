using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MinimercadoAlfredo.Models;

namespace MinimercadoAlfredo.ViewModels
{
    public class CreateSaleVM
    {
        public Sale Sale { get; set; }

        public ICollection<Customer> Customers { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}