using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TiendaVirtual.Models
{
    public class PedidoMetadata
    {
        [Display(Name = "Fecha de Compra")]
        public Nullable<System.DateTime> FechaCompra { get; set; }
        [Display(Name = "Importe (€)")]
        public decimal PrecioTotal { get; set; }
    }
}