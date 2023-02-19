using DepremYardimlari.Web.Models;
using DepremYardimlari.Web.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace DepremYardimlari.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index(string sektor="")
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string contentRootPath = _webHostEnvironment.ContentRootPath;

            string path = "";
            path = Path.Combine(webRootPath, "aids.json");
            var content=System.IO.File.ReadAllText(path);
            //or path = Path.Combine(contentRootPath , "wwwroot" ,"CSS" );
            //var content =Server
            if (String.IsNullOrEmpty(sektor))
                ViewBag.Data = JsonConvert.DeserializeObject<List<Aid>>(content);
            else
            {
                var aids= JsonConvert.DeserializeObject<List<Aid>>(content)
                    .ToList().Where(p => p.Sektor == sektor).ToList();
                ViewBag.Data = aids;
            }
             
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
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Contact(string name, string company, string email, string message, string messageFrom)
        {
            // accountService.SaveContactData(name, company, email, message,Request.ServerVariables["remote_addr"],messageFrom);
            MailService.SendEmail("oguz@quikmotion.com", "AidTurkey Contact Request",
                "Name:" + name + "<br>" +
                 "Email:" + email + "<br>" +
                "Company:" + company + "<br>" +
                "Message:" + message + "<br>" +
                "Message from:" + messageFrom

                );
            return Json(new { success = true });
        }
    }
}