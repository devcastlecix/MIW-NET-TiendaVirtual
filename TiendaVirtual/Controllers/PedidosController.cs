using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TiendaVirtual.Models;

namespace TiendaVirtual.Controllers
{
    [Authorize]
    public class PedidosController : Controller
    {
        private TiendaVirtualDBEntities db = new TiendaVirtualDBEntities();

        // GET: Pedidos
        [Authorize]
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return View("Administrador", db.Pedidos.OrderByDescending(p => p.Id).ToList());                
            }
            List<Pedido> pedidos = new List<Pedido>();
            pedidos = db.Pedidos.OrderByDescending(p => p.Id).ToList()
                .FindAll(pedido => pedido.Usuario == User.Identity.Name);
            return View("Usuario", pedidos);
        }
        [Authorize]
        public ActionResult Details(int id)
        {
            Pedido pedido = db.Pedidos.Find(id);

            if (pedido == null)
            {
                return HttpNotFound();
            }

            return View(pedido);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}