using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MinimercadoAlfredo.Models
{
    public class Trademark
    {
        [Key]
        public int IdTrademark { get; set; }

        [Display(Name = "Marca")]
        [Required(ErrorMessage = "Campo Obligatorio")]
        public string TrademarkName { get; set; }

        [DataType(DataType.MultilineText)]
        public string TrademarkDescription { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}