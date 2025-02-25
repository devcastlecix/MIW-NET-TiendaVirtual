
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TiendaVirtual.Models;

namespace TiendaVirtual.Controllers
{
    public class ProductosController : Controller
    {
        private TiendaVirtualDBEntities db = new TiendaVirtualDBEntities();
        // GET: Productos
        public async Task<ActionResult> Index(CarritoCompra carrito)
        {
            if (User.IsInRole("Admin"))
            {
                return View("Administracion/Index", db.ProductosLibros.ToList());
            }

            ViewBag.CantidadCarritoCompra = CalcularCantidadCarritoCompra(carrito);

            string fullUrl = $"{Request.Url.Scheme}://{Request.Url.Authority}/api/producto";
            HttpResponseMessage response =
                await GlobalApiClient.WebAPIClient.GetAsync(fullUrl);                        
            if (response.IsSuccessStatusCode)
            {
                var productos = await response.Content.ReadAsAsync<IEnumerable<ProductoLibro>>();
                return View("Catalogo", productos);
            }
            
            return View("Catalogo", new List<ProductoLibro>());
        }
        private int CalcularCantidadCarritoCompra(CarritoCompra carrito) {
            int cantidad = 0;            
            carrito.ToList().ForEach(x => { cantidad += x.CantidadDisponible; });
            return cantidad;
        }
        #region Administracion
        // GET: Productos/Details/5        
        public ActionResult Details(int? id, string view)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductoLibro producto = db.ProductosLibros.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            ViewBag.View = view;
            return View("Administracion/Details", producto);
        }

        // GET: Productos/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            InitBeforeViewCreate();
            return View("Administracion/Create");
        }

        // POST: Productos/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "Id,Nombre,Descripcion,ISBN,CantidadDisponible,ImagenVinculada,Precio")] 
            ProductoLibro producto)
        {
            if (!ModelState.IsValid)
            {
                InitBeforeViewCreate();
                TempData["ValidationMessage"] = "No se pudo registrar, tiene campos inválidos.";
                return View("Administracion/Create", producto);
            }            

            string validations = "";
            if (db.ProductosLibros.Where(x => x.Nombre == producto.Nombre).Any())
            {
                validations += "Ya existe un producto con el mismo nombre<br>";
            }
            if (db.ProductosLibros.Where(x => x.ISBN == producto.ISBN).Any())
            {
                validations += "Ya existe un producto con el mismo ISBN<br>";
            }
            if (!string.IsNullOrEmpty(validations))
            {
                InitBeforeViewCreate();
                TempData["ValidationMessage"] = validations;
                return View("Administracion/Create", producto);
            }
            db.ProductosLibros.Add(producto);
            db.SaveChanges();
            TempData["OkMessage"] = "Producto registrado exitosamente.";
            return RedirectToAction("Index");
        }
        private void InitBeforeViewCreate() {
            var images = GetImageForCrudProduct();
            
            var url = images != null && images.Any() ? "~/Images/" + images.First() : "~/Content/Images/default.jpg";
            ViewBag.UrlDefault = url;
        }

        private List<String> GetImageForCrudProduct()
        {
            var images = Directory.GetFiles(Server.MapPath("~/Images"))
                      .Select(Path.GetFileName)
                      .ToList();
            ViewBag.Images = new SelectList(images);            
            return images;
        }

        // GET: Productos/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductoLibro producto = db.ProductosLibros.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            GetImageForCrudProduct();
            return View("Administracion/Edit", producto);
        }
        
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(
            [Bind(Include = "Id,Nombre,Descripcion,ISBN,CantidadDisponible,ImagenVinculada,Precio")]
            ProductoLibro producto)
        {
            if (!ModelState.IsValid)
            {
                GetImageForCrudProduct();
                TempData["ValidationMessage"] = "No se pudo modificar, tiene campos inválidos.";
                return View("Administracion/Edit", producto);                
            }

            string validations = "";
            if (db.ProductosLibros.Where(x => x.Nombre == producto.Nombre && x.Id != producto.Id).Any())
            {
                validations += "Ya existe otro producto con el mismo nombre<br>";
            }
            if (db.ProductosLibros.Where(x => x.ISBN == producto.ISBN && x.Id != producto.Id).Any())
            {
                validations += "Ya existe otro producto con el mismo ISBN<br>";
            }
            if (!string.IsNullOrEmpty(validations))
            {
                GetImageForCrudProduct();
                TempData["ValidationMessage"] = validations;
                return View("Administracion/Edit", producto);
            }
            db.Entry(producto).State = EntityState.Modified;
            
            StockAlerta stock = db.StocksAlerta.Find(producto.Id);
            if (stock != null)
            {                
                stock.ReStock = producto.CantidadDisponible >= 2;
                if (producto.CantidadDisponible < 2) {
                    stock.FechaUltimaAlerta = DateTime.Now;
                    stock.ReStock = false;
                } else stock.ReStock = true;
                db.Entry(stock).State = EntityState.Modified;
            }
            else if(producto.CantidadDisponible < 2){
                stock = new StockAlerta();
                stock.ProductoLibro = producto;
                stock.ReStock = false;
                stock.FechaUltimaAlerta = DateTime.Now;
                db.StocksAlerta.Add(stock);
            }
            db.SaveChanges();
            
            TempData["OkMessage"] = "Producto modificado exitosamente.";
            return RedirectToAction("Index");
        }

        // GET: Productos/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductoLibro producto = db.ProductosLibros.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View("Administracion/Delete", producto);
        }

        // POST: Productos/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductoLibro producto = db.ProductosLibros.Find(id);
            if (producto == null) {
                TempData["ErrorMessage"] = "Producto a eliminar ya no existía.";
                return RedirectToAction("Index");
            }
            db.ProductosLibros.Remove(producto);
            db.SaveChanges();
            TempData["OkMessage"] = "Producto eliminado exitosamente.";
            return RedirectToAction("Index");
        }
        #endregion

        #region Carrito Compras
        // GET: Productos/Carrito
        [Authorize(Roles = "User")]
        public ActionResult Carrito(CarritoCompra carrito)
        {
            decimal totalImporte = 0;            
            totalImporte = carrito != null && carrito.Any() ? carrito.Sum(p => p.CantidadDisponible * p.Precio):0;
            ViewBag.TotalImporte = totalImporte;
            return View(carrito);
        }
        
        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarAlCarrito(int id, int cantidad, CarritoCompra cc)
        {
            if (cantidad <= 0)
            {
                TempData["ValidationMessage"] = "El cantidad de seleccionados de un producto debe ser mayor que 0";
                return RedirectToAction("Index");
            }

            ProductoLibro producto = db.ProductosLibros.Find(id);
            var productoEnCarrito = cc.FirstOrDefault(p => p.Id == id);            

            if (producto == null)
            {
                string nombre = "";
                if (productoEnCarrito !=null && 
                    !string.IsNullOrEmpty(productoEnCarrito.Nombre)) {
                    nombre = "<strong><i>" + productoEnCarrito.Nombre + "</i></strong>";
                }
                TempData["ValidationMessage"] = "El producto " + nombre + " ya fue eliminado del catálogo.";
                return RedirectToAction("Index");
            }
            
            if (productoEnCarrito != null)
            {

                int cantidadDisponible = productoEnCarrito.CantidadDisponible + cantidad;
                if (cantidadDisponible > producto.CantidadDisponible) {
                    string msg = $@"Su cantidad disponible es <strong>{producto.CantidadDisponible}</strong> unidad(es) del producto <strong><i>{producto.Nombre}</i></strong> <br>
                    Pero la cantidad total sería de {cantidadDisponible} unidad(es), sobrepasaria de la cantidad disponible. <br>
                    Revise su carrito de compra, la cantidad de unidades seleccionados.";
                    TempData["ValidationMessage"] = msg;
                    return RedirectToAction("Index");
                }
                productoEnCarrito.CantidadDisponible += cantidad;
            }
            else
            {                
                if (cantidad > producto.CantidadDisponible)
                {
                    string msg = $@"Su cantidad disponible es <strong>{producto.CantidadDisponible}</strong> unidad(es) del producto <strong><i>{producto.Nombre}</i></strong> <br>
                    Pero la cantidad total sería de {cantidad} unidad(es), sobrepasaria de la cantidad disponible.";                    
                    TempData["ValidationMessage"] = msg;
                    return RedirectToAction("Index");
                }
                producto.CantidadDisponible = cantidad;
                cc.Add(producto);
            }
            TempData["OkMessage"] = "Se agregó <strong>"+cantidad+"</strong> unidad(es) del producto <strong><i>" + producto.Nombre + "</i></strong> al carrito de compra.";
            return RedirectToAction("Index");
        }
        
        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarSeleccionadoCarrito(int id, int cantidad, CarritoCompra cc)
        {
            if (cantidad <= 0)
            {
                TempData["ValidationMessage"] = "El cantidad a eliminar seleccionados de producto debe ser mayor que 0";
                return RedirectToAction("Index");
            }
            ProductoLibro producto = null;
            foreach (var p in cc)
            {
                if (p.Id == id) {
                    producto = p;
                    break;
                }                    
            }
            if (producto == null) {
                TempData["ValidationMessage"] = "El producto <strong><i>" + producto.Nombre + "</i></strong> ya fue eliminado del carrito de compra.";
                return RedirectToAction("Carrito");
            }
            if (producto.CantidadDisponible < cantidad)
            {
                TempData["ValidationMessage"] = "La cantidad a eliminar supera la cantidad del carrito, si deseas eliminar todos debes colocas la misma cantidad del carrito.Producto: <strong><i>" + producto.Nombre+"</i></strong>";
                return RedirectToAction("Carrito");
            }

            producto.CantidadDisponible -= cantidad;
            if (producto.CantidadDisponible <= 0)
            {
                TempData["OkMessage"] = "Se eliminó el producto <strong><i>" + producto.Nombre+"</i></strong> del carrito de compra.";
                cc.Remove(producto);
            }
            else {
                TempData["OkMessage"] = "Se quitó <b>"+cantidad+ "</b> unidad(es) del producto <strong><i>" + producto.Nombre+"</i></strong> del carrito de compra";
            }
            return RedirectToAction("Carrito");
        }
        
        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VolcarCarrito(CarritoCompra carrito)
        {
            Pedido pedido = new Pedido();
            List<Venta> ventas = new List<Venta>();
            string validaciones = "";
            foreach (ProductoLibro producto in carrito)
            {
                if (producto.CantidadDisponible > 0)
                {                                        
                    pedido.Usuario = User.Identity.Name;
                    pedido.FechaCompra = DateTime.Now;
                    decimal subtotal = producto.CantidadDisponible * producto.Precio;
                    pedido.PrecioTotal = pedido.PrecioTotal + subtotal;

                    ProductoLibro produdctoDb = db.ProductosLibros.Find(producto.Id);
                    if (produdctoDb == null)
                    {
                        validaciones += $"El producto <strong><i>{producto.Nombre}</i></strong> fue eliminado del catalogo";
                        continue;
                    }
                    if (producto.CantidadDisponible > produdctoDb.CantidadDisponible) {
                        validaciones += $@"El producto <strong><i>{produdctoDb.Nombre}</i></strong> 
                    con <b>{produdctoDb.CantidadDisponible}</b> unidad(es), no tiene stock para {producto.CantidadDisponible} solicitado.
                    ";
                        continue;
                    }
                    produdctoDb.CantidadDisponible -= producto.CantidadDisponible;

                    Venta venta = new Venta();                    
                    venta.ProductoLibro = produdctoDb;
                    venta.Cantidad = producto.CantidadDisponible;
                    venta.Subtotal = subtotal;                                        
                    ventas.Add(venta);
                    
                    if (produdctoDb.CantidadDisponible < 2)
                    {
                        StockAlerta stock = db.StocksAlerta.Find(producto.Id);
                        if (stock == null)
                        {
                            stock = new StockAlerta();
                            stock.ProductoLibro = produdctoDb;
                            stock.ReStock = false;
                            stock.FechaUltimaAlerta = DateTime.Now;
                            db.StocksAlerta.Add(stock);
                        }
                        else {
                            stock.ReStock = false;
                            stock.FechaUltimaAlerta = DateTime.Now;
                            db.Entry(stock).State = EntityState.Modified;
                        }                                                
                    }
                }
            }
            
            if (!string.IsNullOrEmpty(validaciones))
            {
                TempData["ValidationMessage"] = validaciones;
                return RedirectToAction("Carrito");
            }

            if (ventas.Count == 0) {
                TempData["ValidationMessage"] = "No se ha volcado ningun producto";
                return RedirectToAction("Carrito");
            }
            db.Pedidos.Add(pedido);
            db.SaveChanges();            
            
            ventas.ForEach(venta => {
                venta.Pedido = pedido;
                db.Ventas.Add(venta);
            });
            db.SaveChanges();

            carrito.Clear();
            return RedirectToAction("Index", "Pedidos");
        }
        #endregion
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
