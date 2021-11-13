using Microsoft.AspNetCore.Mvc;
using Inventario.Models;
using Inventario.Services;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;

namespace Inventario.Controllers
{
    public class InicioController : Controller{
        private readonly IOperaciones db;
        public InicioController(IOperaciones db)
        {
            this.db = db;
        }
        public IActionResult Login(){
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string usuario, string password){

            Usuarios u = db.Login(usuario, password);
            if(u == null)
            {
                ViewBag.Error = "Usuario y/o password incorrecto";
                return View();
            }
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("username",u.Usuario));
            claims.Add(new Claim(ClaimTypes.Role, u.Rol.Rol));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, u.Usuario));
            ClaimsIdentity identityClaims = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal claimPrincipal = new ClaimsPrincipal(identityClaims);
            await HttpContext.SignInAsync(claimPrincipal);
            return RedirectToAction("Inicio","Productos");
        }

        public async Task<IActionResult> CerrarSesion(){
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult NoAutorizado(){
            return View();
        }
    }
}