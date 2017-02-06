using System;
using System.Collections.Generic;
using MinimercadoAlfredo.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimercadoAlfredo.ViewModels
{
    class CatalogVM
    {
        
        public ICollection<Product> Products { get; set; }
        public ICollection<Category> Categories { get; set; } 
    }
}
