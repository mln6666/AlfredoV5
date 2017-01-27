using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MinimercadoAlfredo.Models
{
    public enum SaleState

    {
        [Display(Name = "Venta en curso")]
        Pendiente = 0,

        [Display(Name = "Venta Finalizada")]
        Finalizada = 1,

        
    }
}