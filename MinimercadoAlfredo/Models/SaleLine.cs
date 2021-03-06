﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MinimercadoAlfredo.Models
{
    public class SaleLine
    {
        [Key]
        public int IdSaleLine { get; set; }

        public int IdProduct { get; set; } //Clave Foránea Producto

        public virtual Product Product { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal LinePrice { get; set; }

        public decimal LineDiscount { get; set; }

        public decimal LineQuantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal LineTotal { get; set; }

        public decimal? Return {get; set; }

        public decimal LineTotalReturn { get; set; }

        public int IdSale { get; set; }

        public virtual Sale Sale { get; set; }




        //[Key]
        //public int IdSaleLine { get; set; }

        //public int Discount { get; set; }

        //public float SaleQuantity { get; set; }

        //public float LineTotal { get; set; }

        //public int IdSale { get; set; }

        //public virtual Sale Sale { get; set; }

        //public int IdProduct { get; set; } //Clave Foránea Producto

        //public virtual Product Product { get; set; }



    }
}