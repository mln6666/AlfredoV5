using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MinimercadoAlfredo.Models
{
    public class Purchase
    {
        [Key]
        public int IdPurchase { get; set; }

        [Display(Name = "Fecha de Compra")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime PurchaseDate { get; set; }

        [Display(Name = "Comentarios")]
        public string Comments { get; set; }

        [Display(Name = "Total")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal? PurchaseTotal { get; set; }

        public int IdProvider { get; set; } //Clave Foránea de Proveedor (Provider)

        public virtual Provider Provider { get; set; }

        public virtual ICollection<PurchaseLine> PurchaseLines { get; set; }
    }
}