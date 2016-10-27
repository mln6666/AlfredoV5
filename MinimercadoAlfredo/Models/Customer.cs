using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MinimercadoAlfredo.Models
{
    public class Customer
    {
        [Key]
        public int IdCustomer { get; set; }

        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "Campo Obligatorio")]
        public string CustomerName { get; set; }

        [Display(Name = "Dirección")]
        public string CustomerAddress { get; set; }

        [Display(Name = "Teléfono")]
        public int? CustomerPhone { get; set; }

        public string CustomerEmail { get; set; }

        public string CuitCuil { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }
    }
}