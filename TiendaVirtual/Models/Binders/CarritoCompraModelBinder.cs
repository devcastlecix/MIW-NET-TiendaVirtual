using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TiendaVirtual.Models.Binders
{
    public class CarritoCompraModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            CarritoCompra cc = (CarritoCompra)controllerContext.HttpContext.Session["cc"];

            if (cc == null)
            {
                cc = new CarritoCompra();
                controllerContext.HttpContext.Session["cc"] = cc;
            }

            return cc;
        }
    }
}