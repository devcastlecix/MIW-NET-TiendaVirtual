//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TiendaVirtual.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class StockAlerta
    {
        public int ProductoId { get; set; }
        public bool ReStock { get; set; }
        public Nullable<System.DateTime> FechaUltimaAlerta { get; set; }
    
        public virtual ProductoLibro ProductoLibro { get; set; }
    }
}
