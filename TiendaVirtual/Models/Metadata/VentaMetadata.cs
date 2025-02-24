using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TiendaVirtual.Models
{
    public class VentaMetadata
    {
        [Display(Name = "Subtotal (€)")]
        public decimal Subtotal { get; set; }
    }
}