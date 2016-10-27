using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MinimercadoAlfredo.Models
{
    public class ProductProvider
    {
        [Key]
        public int IdProductProvider { get; set; }

        public int IdProvider { get; set; }

        public int IdProduct { get; set; }

        public virtual Provider Provider { get; set; }

        public virtual Product Product { get; set; }
    }
}