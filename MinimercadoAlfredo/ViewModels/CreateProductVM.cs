using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MinimercadoAlfredo.ViewModels
{
    public class CreateProductVM
    {
        public string ProductDescription { get; set; }
        public string ProductCategory { get; set; }
        public string ProductTrademark { get; set; }
        public decimal ProductCost { get; set; }
        public decimal ProductWholeSalePrice { get; set; }
        public bool ProductState { get; set; }
        public decimal ProductStock { get; set; }
        public decimal ProductMinimum { get; set; }
    }
}