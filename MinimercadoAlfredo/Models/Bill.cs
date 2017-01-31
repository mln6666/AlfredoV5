using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MinimercadoAlfredo.Models
{
    public class Bill
    {
        [Key]
        public int IdBill { get; set; }

        [Display(Name = "Fecha de Venta")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime SaleDate { get; set; }

        [Display(Name = "Dirección")]
        public string SaleAddress { get; set; }

        [Display(Name = "Descuento")]
        public int? Discount { get; set; }

        [Display(Name = "Obs.")]
        public string Comments { get; set; }

        [Display(Name = "Subtotal")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal? SubTotal { get; set; }

        [Display(Name = "Total Lineas")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal? LinesTotal { get; set; }

        [Display(Name = "Total")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal? SaleTotal { get; set; }

        public int? IdCustomer { get; set; } //Clave Foránea de Cliente (Customer)

        public virtual Customer Customer { get; set; }

        public virtual ICollection<BillLine> BillLines { get; set; }

       
        

    }
}