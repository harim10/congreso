using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Inventario.Services;
using Inventario.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using Microsoft.AspNetCore.Hosting;

namespace Inventario.Controllers
{
    public class ProductosController : Controller
    {

        private readonly IOperaciones db;
        private readonly IWebHostEnvironment env;

        public ProductosController(IOperaciones db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }
        [Authorize]
        public IActionResult Inicio()
        {

            return View(db.GetProductos());
        }
        [Authorize(Roles = "Admin")]
        public IActionResult CrearProducto()
        {
            ViewBag.Id_Categoria = new SelectList(db.GetCategorias(),"Id","Nombre");
            return View();
        }

        [HttpPost]
        public IActionResult CrearProducto(Productos producto, IFormFile Imagen)
        {
            if(producto.Nombre == "Cerveza" || producto.Nombre == "Pilas")
                ModelState.AddModelError("Nombre","No se acepta");
            if (ModelState.IsValid)
            {
                if(Imagen != null){
                     byte [] imagenTemp = null;
                     using(MemoryStream ms = new MemoryStream()){
                         Imagen.CopyTo(ms);
                         imagenTemp = ms.ToArray();
                     }
                     producto.Imagen = imagenTemp;
                }

                db.AddProducto(producto);
                return RedirectToAction("Inicio");
            }
            ViewBag.Id_Categoria = new SelectList(db.GetCategorias(),"Id","Nombre",producto.Id_Categoria);
            return View(producto);
        }

        public IActionResult ActualizarProducto(int id){
            Productos p = db.GetProductoById(id);
            ViewBag.Id_Categoria = new SelectList(db.GetCategorias(),"Id","Nombre",p.Id_Categoria);
            return View(p);
        }

        [HttpPost]
        public IActionResult ActualizarProducto(Productos producto, IFormFile Imagen){
            if (ModelState.IsValid)
            {
                if(Imagen != null){
                     byte [] imagenTemp = null;
                     using(MemoryStream ms = new MemoryStream()){
                         Imagen.CopyTo(ms);
                         imagenTemp = ms.ToArray();
                     }
                     producto.Imagen = imagenTemp;
                }
                db.UpdateProducto(producto);
                return RedirectToAction("Inicio");
            }
            ViewBag.Id_Categoria = new SelectList(db.GetCategorias(),"Id","Nombre",producto.Id_Categoria);
            return View(producto);
        }

        public IActionResult EliminarProducto(int id){
            Productos p = db.GetProductoById(id);
            return View(p);
        }
        [HttpPost]
        [ActionName("EliminarProducto")]
        public IActionResult EliminarProducto2(int id){
            db.DeleteProducto(id);
            return RedirectToAction("Inicio");
        }

        
        public IActionResult GetFoto(int id){
            try
            {
                 Productos p = db.GetProductoById(id);
                 return File(p.Imagen, "imagen/jpeg");
            }
            catch (Exception ex)
            {
                string path = Path.Combine(env.WebRootPath, "imagenes","notfound.jpg");
                return PhysicalFile(path,"image/jpeg");
            }
        }
    }
}