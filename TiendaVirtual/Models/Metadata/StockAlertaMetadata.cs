using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TiendaVirtual.Models
{
    public class StockAlertaMetadata
    {
        [Display(Name = "¿Con Stock?")]
        public bool ReStock { get; set; }
        [Display(Name = "Fecha ultima de la alerta")]
        public Nullable<System.DateTime> FechaUltimaAlerta { get; set; }
    }
}