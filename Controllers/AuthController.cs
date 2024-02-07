using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using API_Complete_ASP.Models;
using Microsoft.EntityFrameworkCore;
using API_Complete_ASP.Database.Services;

namespace API_Complete_ASP.Controllers
{
    public class AuthController : Controller
    {

        private readonly APICompleteContext _context;
        private readonly IUserRepo _Irepo;

        private readonly UserRepo _repo;

        public AuthController(APICompleteContext context, IUserRepo irepo, UserRepo repo)
        {
            _context = context;
            _Irepo = irepo;
            _repo = repo;
        }

        // GET: AuthController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AuthController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AuthController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuthController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        public async Task<IActionResult> ResetPass(int? id)
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
        public async Task<IActionResult> ResetPass(int id, [Bind("IdUser,Users,Email,Password")] User user )
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

        // GET: AuthController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AuthController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }





        public ActionResult btnCorreo_click(object sender, EventArgs e)
        {

            string body = "Para rellenar los datos opcionales entra en el siguiente enlace: " +
                "https://localhost:7294/Home/CreateAddOptionalsClient";

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

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.IdUser == id)).GetValueOrDefault();
        }
    }
}
