using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TiendaVirtual.Models;

namespace TiendaVirtual.Controllers
{
    public class ProductoApiController : ApiController
    {
        private TiendaVirtualDBEntities db = new TiendaVirtualDBEntities();
        // GET api/<controller>
        public IQueryable<ProductoLibro> Get()
        {
            return db.ProductosLibros;
        }

        // GET api/<controller>/5
        [ResponseType(typeof(ProductoLibro))]
        public IHttpActionResult Get(int id)
        {
            ProductoLibro producto = db.ProductosLibros.Find(id);
            if (producto == null)
            {
                return NotFound();
            }

            return Ok(producto);
        }
        // POST: api/Producto
        [ResponseType(typeof(ProductoLibro))]
        public IHttpActionResult Post(ProductoLibro producto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string validations = "";
            if (db.ProductosLibros.Where(x => x.Nombre == producto.Nombre).Any())
            {
                validations += "Ya existe un producto con el mismo nombre | ";
            }
            if (db.ProductosLibros.Where(x => x.ISBN == producto.ISBN).Any())
            {
                validations += "Ya existe un producto con el mismo ISBN | ";
            }
            if (!string.IsNullOrEmpty(validations)) {
                return BadRequest(validations);
            }
            
            db.ProductosLibros.Add(producto);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = producto.Id }, producto);
        }

        // PUT api/<controller>/5
        // PUT: api/Producto/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Put(int id, ProductoLibro producto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != producto.Id)
            {
                return BadRequest("El id no corresponde al producto a modificar");
            }
            string validations = "";
            if (db.ProductosLibros.Where(x => x.Nombre == producto.Nombre && x.Id != producto.Id).Any())
            {
                validations += "Ya existe otro producto con el mismo nombre |";
            }
            if (db.ProductosLibros.Where(x => x.ISBN == producto.ISBN && x.Id != producto.Id).Any())
            {
                validations += "Ya existe otro producto con el mismo ISBN |";
            }
            if (!string.IsNullOrEmpty(validations))
            {
                return BadRequest(validations);
            }

            db.Entry(producto).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/Producto/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Delete(int id)
        {
            ProductoLibro producto = db.ProductosLibros.Find(id);
            if (producto == null)
            {
                return NotFound();
            }

            db.ProductosLibros.Remove(producto);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        private bool ProductoExists(int id)
        {
            return db.ProductosLibros.Count(e => e.Id == id) > 0;
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