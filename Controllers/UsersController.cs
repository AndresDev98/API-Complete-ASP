using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using API_Complete_ASP.Models;
using API_Complete_ASP.Database;
using Humanizer;
using NuGet.Protocol.Core.Types;
using API_Complete_ASP.Database.Services;
using System.Net.Mail;
using System.Net;
using API_Complete_ASP.Models.Dtos;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace API_Complete_ASP.Controllers
{
    public class UsersController : Controller
    {
        private readonly APICompleteContext _context;
        private readonly IUserRepo _Irepo;

        private readonly UserRepo _repo;

        public UsersController(APICompleteContext context, IUserRepo irepo, UserRepo repo)
        {
            _context = context;
            _Irepo = irepo;
            _repo = repo;
        }


        // OBTENER LISTA DE USUARIOS ------------------------------------------------------------------------------- //
        public async Task<IActionResult> Index()
        {
              return _context.Users != null ? 
                          View(await _context.Users.ToListAsync()) :
                          Problem("Entity set 'APICompleteContext.Users'  is null.");
        }


        // DETALLE DE LOS DATOS DE USUARIOS ------------------------------------------------------------------------------- //
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.IdUser == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        // LOGIN ------------------------------------------------------------------------------- //
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            User usuario = _repo.GetByEmail(dto.Email);
            if (usuario == null)
            {

                ViewData["MENSAJE"] = "Email Incorrecto";
                return View();

            }
            else if (!BCrypt.Net.BCrypt.Verify(dto.Password, usuario.Password))
            {

                ViewData["MENSAJE"] = "Contraseña Incorrecta";
                return View();

            }
            else
            {
                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Email);
                Claim claimUserName = new Claim(ClaimTypes.Name, usuario.Name);
                Claim claimEmail = new Claim(ClaimTypes.Email, usuario.Email);
                Claim claimIdUsuario = new Claim("Id", usuario.IdUser.ToString());

                identity.AddClaim(claimUserName);
                identity.AddClaim(claimIdUsuario);
                identity.AddClaim(claimEmail);

                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.Now.AddMinutes(5)
                });

                return RedirectToAction("Index", "Users");
            }
        }


        // CIERRE SESIÓN ------------------------------------------------------------------------------- //
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Response.Cookies.Delete("jwt");
            return RedirectToAction("Index", "Home");
        }


        // REGISTRO ------------------------------------------------------------------------------- //
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)


            };
            if (user != null)
            {
                return RedirectToAction("Login", "Users", _repo.Create(user));
            }
            else
            {

                ViewData["MENSAJE"] = "Usuario o contraseña incorrectos";
                return RedirectToAction("Register", "Users");
            }
        }


        // EDITAR INFO USUARIO ------------------------------------------------------------------------------- //
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUser,Users,Email,Password")] User user)
        {
            if (id != user.IdUser)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.IdUser))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }


        // BORRAR USUARIO ------------------------------------------------------------------------------- //
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.IdUser == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
 

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'APICompleteContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // MIRAR SI EXISTE USUARIO ------------------------------------------------------------------------------- //
        private bool UserExists(int id)
        {
          return (_context.Users?.Any(e => e.IdUser == id)).GetValueOrDefault();
        }



        // ENVIO DE PASSWORD POR CORREO ------------------------------------------------------------------------------- //
        public IActionResult ResetPass()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPass(LoginDto dto)
        {
            User userEmail = _repo.GetByEmail(dto.Email);

            if (userEmail == null)
            {
                ViewData["MENSAJE"] = "Email Incorrecto";
                return View();
            }
            else if (!userEmail.Email.Equals(dto.Email))
            {
                ViewData["MENSAJE"] = "Contraseña Incorrecta";
                return View();
            }
            else
            {
                if (userEmail.Email.Equals(dto.Email))
                {

                    string body = "Para rellenar los datos opcionales entra en el siguiente enlace: " +
                                    "https://localhost:7165/Users/NewPassword";

                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                    smtp.Credentials = new NetworkCredential("desarrolladorapi@gmail.com", "znvr obzi nfra swah");
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;

                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("desarrolladorapi@gmail.com", "Correo Enviado");
                    mail.To.Add(new MailAddress("desarrolladorapi@gmail.com"));
                    mail.Subject = "Mensaje bienvenida";
                    mail.IsBodyHtml = true;
                    mail.Body = body;

                    smtp.Send(mail);
                    Console.WriteLine("Correo enviado");

                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    ViewData["MENSAJE"] = "Login Incorrecto";
                    return RedirectToAction("Login", "Users");
                }
            }
        }


        // NUEVA CONTRASEÑA ------------------------------------------------------------------------------- //
        public IActionResult NewPassword()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewPassword(User user,NewPassword dto)
        {
            User userEmail = _repo.GetByEmail(dto.Email);           

            if (userEmail == null)
            {
                ViewData["MENSAJE"] = "Email Incorrecto";
                return View();
            }
            else if (!userEmail.Email.Equals(dto.Email))
            {
                ViewData["MENSAJE"] = "Contraseña Incorrecta";
                return View();
            }
            else
            {
                if (userEmail.Email.Equals(dto.Email))
                {

                    var dni = new User()
                    {
                        Name = userEmail.Name,
                        Email = userEmail.Email,
                        Password = dto.Password

                    };
                    _context.Users.Update(dni);
                    await _context.SaveChangesAsync();

                    var userddw = await _context.Users.FindAsync(userEmail.IdUser);

                    _context.Users.Remove(userddw);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
            }
            return View();
        }


        // FIN ------------------------------------------------------------------------------- //
    }
}
