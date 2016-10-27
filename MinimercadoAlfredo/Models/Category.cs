using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MinimercadoAlfredo.Models
{
    public class Category
    {
        [Key]
        public int IdCategory { get; set; }

        [Display(Name = "Rubro")]
        [Required(ErrorMessage = "Campo Obligatorio")]
        public string CategoryName { get; set; }

        public string CategoryDescription { get; set; }

        public virtual ICollection<Product> Products { get; set; }

    }
}