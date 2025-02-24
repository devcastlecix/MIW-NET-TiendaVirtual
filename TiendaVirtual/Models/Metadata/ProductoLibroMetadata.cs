using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TiendaVirtual.Models
{
    public class ProductoLibroMetadata
    {
        [Required(ErrorMessage = "El Nombre es requerido.")]
        [StringLength(255, ErrorMessage = "La longitud máxima es de 255 caracteres.")]
        public string Nombre;       
        
        [StringLength(500, ErrorMessage = "La longitud máxima es de 500 caracteres.")]
        public string Descripcion;

        [Required(ErrorMessage = "El ISBN es requerido.")]
        [StringLength(25, ErrorMessage = "La longitud máxima es de 25 caracteres.")]
        [RegularExpression(
    @"(?:ISBN(?:-1[03])?:?\s)?(?=[-\s0-9X]{10,17}$)(?:97[89][-\s]?)?(?:\d[-\s]?){9}[\dX]",
    ErrorMessage = "El ISBN no cumple con el estándar. Ejemplo: ISBN 979-8868804878")]
        public string ISBN;
        [Required(ErrorMessage = "La cantidad disponible es requerido.")]
        [Range(0, 1000, ErrorMessage = "Cantidad disponible debe ser un valor entre 0 y 1000.")]
        [Display(Name = "Cantidad Disponible")]
        public int CantidadDisponible;

        [Required(ErrorMessage = "La imagen vinculada es requerido.")]
        [RegularExpression(@"^.*\.(jpg|png)$",
                    ErrorMessage = "La extensión del archivo debe ser: .jpg o .png")]
        [Display(Name = "Imagen Vinculada")]
        public string ImagenVinculada;

        [Required(ErrorMessage = "El precio es requerido.")]
        [Range(0, 1000, ErrorMessage = "Precio debe ser un valor entre 0 y 1000")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Precio debe ser un número con máximo 2 decimales.")]
        [Display(Name = "Precio (€)")]
        public decimal Precio;        
    }
}