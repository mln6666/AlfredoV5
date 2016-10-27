using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MinimercadoAlfredo.Models
{
    public class Sale
    {
        [Key]
        public int IdSale { get; set; }

        [Display(Name = "Fecha de Venta")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime SaleDate { get; set; }

        public string SaleAddress { get; set; }
        
        public int? Discount { get; set; }

        public string Comments { get; set; }

        [Display(Name = "Total")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal? SaleTotal { get; set; }

        public int IdCustomer { get; set; } //Clave Foránea de Cliente (Customer)

        public virtual Customer Customer { get; set; }

        public virtual ICollection<SaleLine> SaleLines { get; set; }

        public virtual SaleState SaleState { get; set; }
}
}