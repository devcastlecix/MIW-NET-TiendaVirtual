using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaVirtual.Models;

namespace TiendaVirtual.Controllers
{
    public class StocksAlertaController : Controller
    {
        private TiendaVirtualDBEntities db = new TiendaVirtualDBEntities();

        // GET: Pedidos
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.StocksAlerta);
        }
    }
}