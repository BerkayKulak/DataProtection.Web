using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using XSS.Web.Models;

namespace XSS.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private HtmlEncoder _htmlEncoder;
        private JavaScriptEncoder _javaScriptEncoder;
        private UrlEncoder _urlEncoder;

        public HomeController(ILogger<HomeController> logger,JavaScriptEncoder javaScriptEncoder,UrlEncoder urlEncoder, HtmlEncoder htmlEncoder)
        {
            _logger = logger;
            _javaScriptEncoder = javaScriptEncoder;
            _urlEncoder = urlEncoder;
            _htmlEncoder = htmlEncoder;
        }

        public IActionResult CommentAdd()
        {
            HttpContext.Response.Cookies.Append("email","kulakberkay15@gmail.com");
            HttpContext.Response.Cookies.Append("password","1234");

            if (System.IO.File.Exists("comment.txt"))
            {
                ViewBag.comment = System.IO.File.ReadAllLines("comment.txt");

            }

            return View();

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult CommentAdd(string name,string comment)
        {
            string encodeName = _urlEncoder.Encode(name);

            ViewBag.name = name;
            ViewBag.comment = comment;
           
            System.IO.File.AppendAllText("comment.txt", $"{name}-{comment}\n");
            return RedirectToAction("CommentAdd");
        }

        public IActionResult login(string returnUrl = "/")
        {
            TempData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult login(string email, string password )
        {
            string returnUrl = TempData["returnUrl"].ToString();

            // email ve password kontrolü yap

            // benim domaine aitse doğru döner değilse false döner
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);

            }
            else
            {
                return Redirect("/");
            }

        
            
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
