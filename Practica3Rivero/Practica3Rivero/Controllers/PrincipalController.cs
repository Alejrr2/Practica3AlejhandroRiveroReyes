using Practica3Rivero.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Practica3Rivero.Controllers
{
    public class PrincipalController : Controller
    {
        [HttpGet]
        public ActionResult ConsultarProductos()
        {
            using (var context = new PracticaS12Entities())
            {
                var datos = context.Principal.OrderBy(p => p.Estado.ToUpper() != "PENDIENTE").ToList();

                var productos = new List<Principal>();
                foreach (var item in datos)
                {
                    productos.Add(new Principal
                    {
                        Id_Compra = item.Id_Compra,
                        Precio = item.Precio,
                        Saldo = item.Saldo,
                        Descripcion = item.Descripcion,
                        Estado = item.Estado

                    });
                }
                return View(productos);
            }
        }
    }
}