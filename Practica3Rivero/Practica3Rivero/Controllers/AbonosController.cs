using Practica3Rivero.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Practica3Rivero.Controllers
{
    public class AbonosController : Controller
    {
        [HttpGet]
        public ActionResult RegistroAbonos()
        {
            using (var context = new PracticaS12Entities())
            {
                var comprasPendientes = context.Principal.Where(p => p.Estado.ToUpper() == "PENDIENTE").Select(p => new { p.Id_Compra, p.Descripcion }).ToList();

                ViewBag.ComprasPendientes = new SelectList(comprasPendientes, "Id_Compra", "Descripcion");
            }
            ViewBag.SaldoConsultado = false;
            return View();
        }

        [HttpPost]
        public ActionResult RegistroAbonos(long Id_Compra, decimal Monto)
        {
            using (var context = new PracticaS12Entities())
            {
                var comprasPendientes = context.Principal.Where(p => p.Estado.ToUpper() == "PENDIENTE").Select(p => new { p.Id_Compra, p.Descripcion }).ToList();
                ViewBag.ComprasPendientes = new SelectList(comprasPendientes, "Id_Compra", "Descripcion");

                var max = 79228162514264337593543950335M;

                var compra = context.Principal.FirstOrDefault(p => p.Id_Compra == Id_Compra);   

                if (compra == null || Id_Compra <= 0)

                {
                    ViewBag.MensajePantalla = "Por favor seleccione un producto";
                    ViewBag.SaldoConsultado = false;
                    return View();
                }
                if (compra.Id_Compra == null || compra.Saldo == null || compra.Saldo == 0)
                {
                    ViewBag.MensajePantalla = "Por favor consulte algun producto";
                    ViewBag.SaldoConsultado = false;
                    return View();
                }
                if (ViewBag.SaldoConsultado = false)
                {
                    ViewBag.MensajePantalla = "Por favor consulte algun producto";
                    ViewBag.SaldoConsultado = false;
                    return View();
                }

                if (Monto == null || Monto != Math.Round(Monto, 0))
                {
                    ViewBag.MensajePantalla = "El monto del abono no puede contener decimales";
                    ViewBag.SaldoConsultado = false;
                }
                

                if (Monto <= 0 || Monto > compra.Saldo || Monto > max)

                {
                    ViewBag.MensajePantalla = "El monto del abono debe ser mayor a 0 y no exceder el saldo pendiente.";
                    ViewBag.SaldoConsultado = false;
                    return View();
                }

                var registro = new Abonos 
                { 
                    Id_Compra = Id_Compra,
                    Monto = Monto,
                    Saldo = compra.Saldo,
                    Fecha = DateTime.Now 
                };

                context.Abonos.Add(registro);

                compra.Saldo -= Monto;

                if (compra.Saldo == 0) 
                {
                    compra.Estado = "Cancelado"; 
                }

                var respuesta = context.SaveChanges();
                var mensaje = (respuesta > 0 ? "OK" : "ERROR");
                ViewBag.Mensaje = mensaje;
            }

                return RedirectToAction("ConsultarProductos", "Principal");
            }

        [HttpGet]
        public ActionResult ConsultarSaldo()
        {
            ViewBag.SaldoConsultado = false;
            return View();
        }

        [HttpPost]
        public ActionResult ConsultarSaldo(long Id_Compra)
        {
            using (var context = new PracticaS12Entities())
            {
                var comprasPendientes = context.Principal.Where(p => p.Estado.ToUpper() == "PENDIENTE").Select(p => new { p.Id_Compra, p.Descripcion }).ToList();
                ViewBag.ComprasPendientes = new SelectList(comprasPendientes, "Id_Compra", "Descripcion");

                var compra = context.Principal.FirstOrDefault(p => p.Id_Compra == Id_Compra);

                if (compra == null || Id_Compra <= 0)
                {
                    ViewBag.MensajePantalla = "Por favor seleccione un producto";
                    ViewBag.SaldoConsultado = false;
                    return View("RegistroAbonos");
                }

                var abonosModel = new Abonos
                {
                    Id_Compra = Id_Compra,
                    Saldo = compra.Saldo
                };
                ViewBag.SaldoConsultado = true;
                return View("RegistroAbonos", abonosModel);
            }
        }


    }
}
    
