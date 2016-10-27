using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MinimercadoAlfredo.Models
{
    public class Provider
    {
        [Key]
        public int IdProvider { get; set; }

        [Display(Name = "Proveedor")]
        [Required(ErrorMessage = "Campo Obligatorio")]
        public string ProviderName { get; set; }

        [Display(Name = "Dirección")]
        public string ProviderAddress { get; set; }

        [Display(Name = "Teléfono")]
        public int? ProviderPhone { get; set; }

        public string ProviderEmail { get; set; }

        public virtual ICollection<ProductProvider> Products { get; set; }
    }
}